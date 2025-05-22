using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotUIManager : MonoBehaviour
{
    public static QuickSlotUIManager Instance;

    [Header("�÷��̾� �κ��丮 ����")]
    public Inventory playerInventory;

    [Header("�ϴ� ������ UI")]
    public InventorySlotUI[] quickSlots;

    [Header("�Է�")]
    public InputActionReference scrollSlotAction;

    [SerializeField] private PlayerController playerController;

    private int selectedIndex = -1;
    private bool scrollRegistered = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();
    }
    private void OnEnable()
    {
        if (!scrollRegistered)
        {
            scrollSlotAction.action.Enable();
            scrollSlotAction.action.performed += OnScrollSlot;
            scrollRegistered = true;
        }
    }
    private void OnDisable()
    {
        if (scrollRegistered)
        {
            scrollSlotAction.action.performed -= OnScrollSlot;
            scrollSlotAction.action.Disable();
            scrollRegistered = false;
        }
    }
    private void Start()
    {
        Invoke(nameof(RefreshQuickSlot), 0.1f);
        //RefreshQuickSlot(); // ���� �� �ʱ�ȭ
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
            Debug.LogWarning("QuickSlotUIManager: playerInventory ���� �ȵ�");
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
        if (selectedIndex == index) return;

        if (index < 0 || index >= quickSlots.Length) return;

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].SetSelected(i == index);
        }

        selectedIndex = index;
#if UNITY_EDITOR
        Debug.Log($"[QuickSlotUIManager] ������ {index + 1}�� ���õ�");
#endif

        if (playerController != null)
        {
            playerController.ChangeSlot(selectedIndex + 1);
        }
    }
}

