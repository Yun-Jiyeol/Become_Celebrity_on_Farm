using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 전체 UI 관리 스크립트 (30칸 전부 관리)
/// </summary>
public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;
    public Inventory playerInventory; // 플레이어 Inventory 스크립트 참조

    // 슬롯 UI 배열 (총 30칸)
    public InventorySlotUI[] slots;
    // 창고용 인벤토리 데이터 (13번째 칸부터 30번째 칸까지 사용)
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

        Invoke("RefreshUI", 0.1f); // 인벤토리 열릴 때 초기화
                                   ///(근데 Invoke 안 쓰니까 데이터가 처리되는 도중에 Refresh 해버려서 Null오류나서 우선 Invoke해뒀습니다.)
        //RefreshUI(); 
    }

    // 전체 인벤토리 UI 새로고침
    // (플레이어 인벤토리 12칸 + 창고 인벤토리 18칸)
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
        // 첫 클릭 = 선택
        if (selectedSlot == null)
        {
            if (clickedSlot.GetData() == null || clickedSlot.GetData().amount <= 0)
                return;

            selectedSlot = clickedSlot;
        }
        else
        {
            // 같은 슬롯 재클릭 = 선택 해제
            if (selectedSlot == clickedSlot)
            {
                selectedSlot = null;
                return;
            }

            // 이동 처리
            SwapOrMergeItem(selectedSlot, clickedSlot);

            selectedSlot = null;
            RefreshUI();
        }
    }
    private void SwapOrMergeItem(InventorySlotUI from, InventorySlotUI to)
    {
        var fromData = from.GetData();
        var toData = to.GetData();

        // 같은 아이템이면 머지
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
            // 다른 아이템이면 스왑
            (fromData.ItemData_num, toData.ItemData_num) = (toData.ItemData_num, fromData.ItemData_num);
            (fromData.amount, toData.amount) = (toData.amount, fromData.amount);
        }
    }
}

