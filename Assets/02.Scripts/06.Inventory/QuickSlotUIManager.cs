using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotUIManager : MonoBehaviour
{
    public static QuickSlotUIManager Instance;

    [Header("플레이어 인벤토리 참조")]
    public Inventory playerInventory;

    [Header("하단 퀵슬롯 UI")]
    public InventorySlotUI[] quickSlots;

    [Header("입력")]
    public InputActionReference scrollSlotAction;

    private int selectedIndex = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void OnEnable()
    {
        scrollSlotAction.action.Enable();
        scrollSlotAction.action.performed += OnScrollSlot;
    }

    private void OnDisable()
    {
        scrollSlotAction.action.performed -= OnScrollSlot;
        scrollSlotAction.action.Disable();
    }
    private void Start()
    {
        Invoke(nameof(RefreshQuickSlot), 0.1f);
        //RefreshQuickSlot(); // 시작 시 초기화
    }
    private void OnScrollSlot(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<Vector2>().y;

        if (scrollValue == 0) return;

        int direction = scrollValue > 0 ? -1 : 1;
        int newIndex = selectedIndex + direction;

        if (newIndex < 0)
            newIndex = quickSlots.Length - 1;
        else if (newIndex >= quickSlots.Length)
            newIndex = 0;

        SelectSlot(newIndex);
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
    public void SelectSlot(int index)
    {
        if (index < 0 || index >= quickSlots.Length) return;

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].SetSelected(i == index);
        }

        selectedIndex = index;
        Debug.Log($"[QuickSlotUIManager] 퀵슬롯 {index + 1}번 선택됨");
    }
}

