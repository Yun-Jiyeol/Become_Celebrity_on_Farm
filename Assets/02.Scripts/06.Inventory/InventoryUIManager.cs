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

    public InventorySlotUI selectedSlot;

    public GameObject mouseFollowItemObj;
    public Image mouseFollowIcon;
    public TextMeshProUGUI mouseFollowAmount;

    private int tempItemData_num;
    private int tempItemAmount;

    private void Awake() => Instance = this;

    private void Start()
    {
        while (playerInventory.PlayerHave.Count < 30)
            playerInventory.PlayerHave.Add(new Inventory.Inven());

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
            slots[i].SetData(playerInventory.PlayerHave[i]);
        }

        QuickSlotUIManager.Instance?.RefreshQuickSlot();
    }

    public void OnSlotClick(InventorySlotUI clickedSlot)
    {
        var data = clickedSlot.GetData();

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

        if (selectedSlot == clickedSlot)
        {
            var backData = selectedSlot.GetData();
            backData.ItemData_num = tempItemData_num;
            backData.amount = tempItemAmount;

            ClearHoldingItem();
            RefreshUI();
            return;
        }

        var toData = clickedSlot.GetData();

        if (toData.ItemData_num == tempItemData_num && tempItemData_num != 0)
        {
            var itemData = ItemManager.Instance.itemDataReader.itemsDatas[tempItemData_num];
            int canAdd = itemData.Item_Overlap - toData.amount;
            int moveAmount = Mathf.Min(canAdd, tempItemAmount);

            toData.amount += moveAmount;
            tempItemAmount -= moveAmount;

            if (tempItemAmount <= 0)
            {
                ClearHoldingItem();
            }
        }
        else
        {
            int tempNum = toData.ItemData_num;
            int tempAmount = toData.amount;

            toData.ItemData_num = tempItemData_num;
            toData.amount = tempItemAmount;

            if (selectedSlot != null)
            {
                var fromData = selectedSlot.GetData();
                fromData.ItemData_num = tempNum;
                fromData.amount = tempAmount;
            }
            else
            {
                AddItemToInventoryFromMouse(tempNum, tempAmount);
            }

            ClearHoldingItem();
        }

        RefreshUI();
    }

    private void AddItemToInventoryFromMouse(int itemNum, int amount)
    {
        var invenList = playerInventory.PlayerHave;
        for (int i = 0; i < invenList.Count; i++)
        {
            if (invenList[i].ItemData_num == 0)
            {
                invenList[i].ItemData_num = itemNum;
                invenList[i].amount = amount;
                return;
            }
        }
        Debug.LogWarning("빈 슬롯이 없어 아이템 복구 실패");
    }

    private void DropSelectedItem()
    {
        if (!HoldingItem()) return;

        var itemData = ItemManager.Instance.itemDataReader.itemsDatas[tempItemData_num];
        ItemManager.Instance.spawnItem.DropItem(itemData, tempItemAmount, playerTransform.position);

        ClearHoldingItem();
        RefreshUI();
    }

    public void OnClickTrashButton()
    {
        if (!HoldingItem()) return;

        ClearHoldingItem();
        RefreshUI();
    }

    public void OnSlotRightClick(InventorySlotUI clickedSlot)
    {
        var data = clickedSlot.GetData();
        if (HoldingItem() || data == null || data.amount <= 1) return;

        int half = data.amount / 2;
        data.amount -= half;

        SetHoldingItem(data.ItemData_num, half);
        selectedSlot = null;
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

    public void ClearHoldingItem()
    {
        tempItemData_num = 0;
        tempItemAmount = 0;
        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);
    }

    public bool HoldingItem()
    {
        return tempItemAmount > 0 && tempItemData_num != 0;
    }

    public int AddItemToInventory(ItemsData getItem, int amount)
    {
        var invenList = playerInventory.PlayerHave;

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

    public void ForceReturnHoldingItem()
    {
        if (!HoldingItem()) return;

        bool merged = false;

        if (selectedSlot != null)
        {
            var backData = selectedSlot.GetData();

            if (backData.ItemData_num == tempItemData_num)
            {
                backData.amount += tempItemAmount;
                merged = true;
            }
            else if (backData.ItemData_num == 0)
            {
                backData.ItemData_num = tempItemData_num;
                backData.amount = tempItemAmount;
                merged = true;
            }
        }

        if (!merged)
        {
            var invenList = playerInventory.PlayerHave;
            for (int i = 0; i < invenList.Count; i++)
            {
                if (invenList[i].ItemData_num == tempItemData_num)
                {
                    invenList[i].amount += tempItemAmount;
                    merged = true;
                    break;
                }
            }
        }

        if (!merged)
        {
            AddItemToInventoryFromMouse(tempItemData_num, tempItemAmount);
        }

        ClearHoldingItem();
        RefreshUI();
    }
}
