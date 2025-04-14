using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 전체 UI 관리 스크립트 (30칸 전부 관리)
/// </summary>
public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    public Inventory playerInventory; // 플레이어 Inventory 스크립트 참조
    public InventorySlotUI[] slots; // 슬롯 UI 배열 (총 30칸)

    public List<Inventory.Inven> warehouseInven = new List<Inventory.Inven>();
    // 창고용 인벤토리 데이터 (13번째 칸부터 30번째 칸까지 사용)
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Invoke("RefreshUI", 0.1f);
        //RefreshUI(); // 인벤토리 열릴 때 초기화
    }

    /// <summary>
    /// 전체 인벤토리 UI 새로고침
    /// (플레이어 인벤토리 12칸 + 창고 인벤토리 18칸)
    /// </summary>
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < 12)
            {
                // 0 ~ 11번 슬롯 → Player Inventory 연동
                slots[i].SetData(playerInventory.PlayerHave[i]);
            }
            else
            {
                // 12 ~ 29번 슬롯 → 창고 데이터 사용
                slots[i].SetData(warehouseInven[i - 12]);
            }
        }
    }

    // 여기에 아이템 클릭 처리, 이동, 머지 등 메서드 추가 가능
    // 예: OnSlotClick(), SwapItem(), MergeItem() 등
}

