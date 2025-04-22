using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ItemDataReader;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;
    public Inventory playerInventory;
    public RectTransform invenRect;
    public Transform playerTransform;

    public InventorySlotUI[] slots;
    public List<Inventory.Inven> warehouseInven = new List<Inventory.Inven>();

    public InventorySlotUI selectedSlot;

    public GameObject mouseFollowItemObj;
    public Image mouseFollowIcon;
    public TextMeshProUGUI mouseFollowAmount;

    private int tempItemData_num;
    private int tempItemAmount;

    private void Awake() => Instance = this;

    private void Start()
    {
        while (warehouseInven.Count < 18)
            warehouseInven.Add(new Inventory.Inven());

        RefreshUI();
    }

    private void Update()
    {
        if (mouseFollowItemObj.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            mouseFollowItemObj.transform.position = mousePos + new Vector3(5, -5, 0);
        }

        if (HoldingItem() && Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(invenRect, Input.mousePosition))
            {
                DropSelectedItem();
            }
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < 12)
                slots[i].SetData(playerInventory.PlayerHave[i]);
            else
                slots[i].SetData(warehouseInven[i - 12]);
        }

        QuickSlotUIManager.Instance?.RefreshQuickSlot();
    }

    public void OnSlotClick(InventorySlotUI clickedSlot)
    {
        var data = clickedSlot.GetData();

        // 들고 있는 아이템이 없는 경우
        if (!HoldingItem())
        {
            if (data == null || data.amount <= 0) return;

            selectedSlot = clickedSlot;
            SetHoldingItem(data.ItemData_num, data.amount);
            data.ItemData_num = 0;
            data.amount = 0;
            RefreshUI();
            return;
        }

        // 아이템 병합 또는 스왑 시도
        bool success = SwapOrMergeItem(clickedSlot);

        if (!success)
        {
            // 실패 시 → 새 슬롯에 들고 있는 아이템 넣기
            data.ItemData_num = tempItemData_num;
            data.amount = tempItemAmount;
        }

        selectedSlot = null;
        tempItemData_num = 0;
        tempItemAmount = 0;
        mouseFollowItemObj.SetActive(false);
        RefreshUI();
    }

    private bool SwapOrMergeItem(InventorySlotUI toSlot)
    {
        var toData = toSlot.GetData();

        // 병합
        if (toData.ItemData_num == tempItemData_num && tempItemData_num != 0)
        {
            var itemData = ItemManager.Instance.itemDataReader.itemsDatas[tempItemData_num];
            int canAdd = itemData.Item_Overlap - toData.amount;
            int moveAmount = Mathf.Min(canAdd, tempItemAmount);

            toData.amount += moveAmount;
            tempItemAmount -= moveAmount;

            return tempItemAmount <= 0;
        }

        // 빈 슬롯
        if (toData.amount <= 0)
        {
            toData.ItemData_num = tempItemData_num;
            toData.amount = tempItemAmount;
            return true;
        }

        // 스왑
        if (selectedSlot == null) return false;

        var fromData = selectedSlot.GetData();
        int tempNum = toData.ItemData_num;
        int tempAmount = toData.amount;

        toData.ItemData_num = tempItemData_num;
        toData.amount = tempItemAmount;

        fromData.ItemData_num = tempNum;
        fromData.amount = tempAmount;

        return true;
    }

    private void DropSelectedItem()
    {
        if (!HoldingItem())
        {
            Debug.Log("들고 있는 아이템 정보 없음");
            return;
        }

        var itemData = ItemManager.Instance.itemDataReader.itemsDatas[tempItemData_num];
        ItemManager.Instance.spawnItem.DropItem(itemData, tempItemAmount, playerTransform.position);

        tempItemData_num = 0;
        tempItemAmount = 0;
        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);
        RefreshUI();
    }

    public void OnClickTrashButton()
    {
        if (!HoldingItem()) return;

        tempItemData_num = 0;
        tempItemAmount = 0;
        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);
        RefreshUI();
    }

    public void OnSlotRightClick(InventorySlotUI clickedSlot)
    {
        var data = clickedSlot.GetData();
        if (HoldingItem() || data == null || data.amount <= 1) return;

        int half = data.amount / 2;
        data.amount -= half;

        selectedSlot = clickedSlot;
        SetHoldingItem(data.ItemData_num, half);
        RefreshUI();
    }

    public void SetHoldingItem(int itemNum, int amount)
    {
        tempItemData_num = itemNum;
        tempItemAmount = amount;

        if (ItemManager.Instance.itemDataReader.itemsDatas.TryGetValue(itemNum, out var itemData))
        {
            mouseFollowIcon.sprite = itemData.Item_sprite;
            mouseFollowAmount.text = amount.ToString();
            mouseFollowItemObj.SetActive(true);
        }
    }

    public bool HoldingItem()
    {
        return tempItemAmount > 0 && tempItemData_num != 0;
    }

    public int AddItemToWarehouse(ItemsData getItem, int amount)
    {
        return AddItemToInventory(warehouseInven, getItem, amount);
    }

    private int AddItemToInventory(List<Inventory.Inven> invenList, ItemsData getItem, int amount)
    {
        for (int i = 0; i < invenList.Count; i++)
        {
            if (invenList[i].ItemData_num == getItem.Item_num || invenList[i].ItemData_num == 0)
            {
                invenList[i].ItemData_num = getItem.Item_num;
                int canAdd = getItem.Item_Overlap - invenList[i].amount;
                int moveAmount = Mathf.Min(canAdd, amount);

                invenList[i].amount += moveAmount;
                amount -= moveAmount;

                if (amount <= 0)
                    return 0;
            }
        }
        return amount;
    }
}
