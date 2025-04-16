using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ItemDataReader;


// �κ��丮 ��ü UI ���� ��ũ��Ʈ (30ĭ ���� ����)
public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;
    public Inventory playerInventory; // �÷��̾� Inventory ��ũ��Ʈ ����
    public RectTransform invenRect;
    public Transform playerTransform;

    // ���� UI �迭 (�� 30ĭ)
    public InventorySlotUI[] slots;

    // â��� �κ��丮 ������ (13��° ĭ���� 30��° ĭ���� ���)
    public List<Inventory.Inven> warehouseInven = new List<Inventory.Inven>();

    public InventorySlotUI selectedSlot;

    public GameObject mouseFollowItemObj;      // ���콺 ����ٴ� UI
    public Image mouseFollowIcon;              // ������ �̹���
    public TextMeshProUGUI mouseFollowAmount;  // ���� ǥ��

    // �ӽ� ����
    private int tempItemData_num;  
    private int tempItemAmount;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        while (warehouseInven.Count < 18)
            warehouseInven.Add(new Inventory.Inven());

        //Invoke("RefreshUI", 0.1f); // �κ��丮 ���� �� �ʱ�ȭ
        RefreshUI();
    }
    private void Update()
    {
        if (mouseFollowItemObj.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            mouseFollowItemObj.transform.position = mousePos + new Vector3(5, -5, 0); // ���콺 ������ �Ʒ�
        }

        // ������ ��� �ִ� ���� + ���콺 ��Ŭ��
        if (selectedSlot != null && Input.GetMouseButtonDown(0))
        {
            // �κ��丮 RectTransform �ȿ� ���콺 �ִ��� �˻�
            if (!RectTransformUtility.RectangleContainsScreenPoint(invenRect, Input.mousePosition))
            {
                // �ۿ��� �������ϱ� ������ ���
                DropSelectedItem();
            }
        }
    }

    // ��ü �κ��丮 UI ���ΰ�ħ
    // (�÷��̾� �κ��丮 12ĭ + â�� �κ��丮 18ĭ)
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < 12)
                slots[i].SetData(playerInventory.PlayerHave[i]);
            else
                slots[i].SetData(warehouseInven[i - 12]);
        }

    }

    public void OnSlotClick(InventorySlotUI clickedSlot)
    {
        // ù Ŭ�� �� ������ ���
        if (selectedSlot == null)
        {
            var data = clickedSlot.GetData();
            if (data == null || data.amount <= 0)
                return;

            selectedSlot = clickedSlot;

            // ���콺 ����ٴ� ������ ����
            var itemData = ItemManager.Instance.itemDataReader.itemsDatas[data.ItemData_num];
            mouseFollowIcon.sprite = itemData.Item_sprite;
            mouseFollowAmount.text = data.amount.ToString();
            mouseFollowItemObj.SetActive(true);

            // ���� ���� ������ ���� ����
            tempItemData_num = data.ItemData_num;
            tempItemAmount = data.amount;

            // ���� ����
            data.ItemData_num = 0;
            data.amount = 0;

            RefreshUI();
            return;
        }

        // �ι�° Ŭ��
        bool success = SwapOrMergeItem(selectedSlot, clickedSlot);

        if (!success)
        {
            // ���־����� ���� �ڸ��� ����
            selectedSlot.GetData().ItemData_num = tempItemData_num;
            selectedSlot.GetData().amount = tempItemAmount;
        }

        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);
        RefreshUI();
    }
    public void OnClickTrashButton()
    {
        if (selectedSlot == null || tempItemAmount <= 0)
        {
            Debug.Log("������ �������� ����.");
            return;
        }

        // �׳� ��� �ִ� �����͸� ����
        tempItemData_num = 0;
        tempItemAmount = 0;

        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);

        RefreshUI();
    }
    private bool SwapOrMergeItem(InventorySlotUI from, InventorySlotUI to)
    {
        var fromData = from.GetData();
        var toData = to.GetData();

        // ���� ������ = Merge
        if (toData.ItemData_num == tempItemData_num && tempItemData_num != 0)
        {
            var itemData = ItemManager.Instance.itemDataReader.itemsDatas[tempItemData_num];

            int canAdd = itemData.Item_Overlap - toData.amount;
            int moveAmount = Mathf.Min(canAdd, tempItemAmount);

            toData.amount += moveAmount;
            tempItemAmount -= moveAmount;

            return tempItemAmount <= 0;
        }
        // �� ���� = �׳� �ֱ�
        else if (toData.amount <= 0)
        {
            toData.ItemData_num = tempItemData_num;
            toData.amount = tempItemAmount;
            return true;
        }
        // �ٸ� ������ = �ڸ� Swap
        else
        {
            // Swap ó��
            int tempNum = toData.ItemData_num;
            int tempAmount = toData.amount;

            // toData = ����ִ��� �ֱ�
            toData.ItemData_num = tempItemData_num;
            toData.amount = tempItemAmount;

            // fromData = ���� toData ������ �ֱ�
            fromData.ItemData_num = tempNum;
            fromData.amount = tempAmount;

            return true;
        }
    }

    // â�� �κ��丮�� ������ �߰�
    public int AddItemToWarehouse(ItemDataReader.ItemsData getItem, int amount)
    {
        return AddItemToInventory(warehouseInven, getItem, amount);
    }

    // ���� ������ �߰� ����
    private int AddItemToInventory(List<Inventory.Inven> invenList, ItemDataReader.ItemsData getItem, int amount)
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
    private void DropSelectedItem()
    {
        if (tempItemData_num == 0 || tempItemAmount <= 0)
        {
            Debug.Log("��� �ִ� ������ ���� ����");
            return;
        }

        var itemData = ItemManager.Instance.itemDataReader.itemsDatas[tempItemData_num];
        Debug.Log($"��� �õ�: {itemData.Item_name}, ����: {tempItemAmount}");

        // ���
        ItemManager.Instance.spawnItem.DropItem(itemData, tempItemAmount, playerTransform.position);

        // �ʱ�ȭ
        tempItemData_num = 0;
        tempItemAmount = 0;
        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);

        RefreshUI();
    }
}

