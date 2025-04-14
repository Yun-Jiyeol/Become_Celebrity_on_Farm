using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �κ��丮 ��ü UI ���� ��ũ��Ʈ (30ĭ ���� ����)
/// </summary>
public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;
    public Inventory playerInventory; // �÷��̾� Inventory ��ũ��Ʈ ����

    // ���� UI �迭 (�� 30ĭ)
    public InventorySlotUI[] slots;
    // â��� �κ��丮 ������ (13��° ĭ���� 30��° ĭ���� ���)
    public List<Inventory.Inven> warehouseInven = new List<Inventory.Inven>();

    public InventorySlotUI selectedSlot;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        while (warehouseInven.Count < 18)
            warehouseInven.Add(new Inventory.Inven());

        Invoke("RefreshUI", 0.1f); // �κ��丮 ���� �� �ʱ�ȭ
                                   ///(�ٵ� Invoke �� ���ϱ� �����Ͱ� ó���Ǵ� ���߿� Refresh �ع����� Null�������� �켱 Invoke�ص׽��ϴ�.)
        //RefreshUI(); 
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
        // ù Ŭ�� = ����
        if (selectedSlot == null)
        {
            if (clickedSlot.GetData() == null || clickedSlot.GetData().amount <= 0)
                return;

            selectedSlot = clickedSlot;
        }
        else
        {
            // ���� ���� ��Ŭ�� = ���� ����
            if (selectedSlot == clickedSlot)
            {
                selectedSlot = null;
                return;
            }

            // �̵� ó��
            SwapOrMergeItem(selectedSlot, clickedSlot);

            selectedSlot = null;
            RefreshUI();
        }
    }
    private void SwapOrMergeItem(InventorySlotUI from, InventorySlotUI to)
    {
        var fromData = from.GetData();
        var toData = to.GetData();

        // ���� �������̸� ����
        if (fromData.ItemData_num == toData.ItemData_num && fromData.ItemData_num != 0)
        {
            var itemData = ItemManager.Instance.itemDataReader.itemsDatas[fromData.ItemData_num];

            int canAdd = itemData.Item_Overlap - toData.amount;
            int moveAmount = Mathf.Min(canAdd, fromData.amount);

            toData.amount += moveAmount;
            fromData.amount -= moveAmount;

            if (fromData.amount <= 0)
                fromData.ItemData_num = 0;
        }
        else
        {
            // �ٸ� �������̸� ����
            (fromData.ItemData_num, toData.ItemData_num) = (toData.ItemData_num, fromData.ItemData_num);
            (fromData.amount, toData.amount) = (toData.amount, fromData.amount);
        }
    }
}

