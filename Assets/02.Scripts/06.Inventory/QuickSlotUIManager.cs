using UnityEngine;

public class QuickSlotUIManager : MonoBehaviour
{
    public static QuickSlotUIManager Instance;

    [Header("플레이어 인벤토리 참조")]
    public Inventory playerInventory;

    [Header("하단 퀵슬롯 UI")]
    public InventorySlotUI[] quickSlots;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(RefreshQuickSlot), 0.1f);
        //RefreshQuickSlot(); // 시작 시 초기화
    }

    public void RefreshQuickSlot()
    {
        if (playerInventory == null || playerInventory.PlayerHave == null)
        {
            Debug.LogWarning("QuickSlotUIManager: playerInventory 연결 안됨");
            return;
        }

        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (i < playerInventory.PlayerHave.Count && quickSlots[i] != null)
            {
                quickSlots[i].SetData(playerInventory.PlayerHave[i]);
            }
        }
    }
}

