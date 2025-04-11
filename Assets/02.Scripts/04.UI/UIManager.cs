using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Prefabs")]
    public GameObject characterChoiceUIPrefab;

    [Header("Parent")]
    public Transform uiRoot; //캔버스 밑의 패널 등

    [Header("Ingame UI")]
    public GameObject inventoryUI; // <- 인벤토리 UI 연결

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("다수의 UIManager 존재!");
            Destroy(gameObject);
            return;
        }
    }

    public void ShowCharacterChoiceUI()
    {
        GameObject ui = Instantiate(characterChoiceUIPrefab, uiRoot);
        CharacterChoice choice = ui.GetComponent<CharacterChoice>();
        choice.Setup();
    }

    // 인벤토리 열고 닫기 Toggle
    public void ToggleInventoryUI()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("인벤토리 UI가 연결 안됨");
            return;
        }

        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive);
    }

    // 인벤토리 열려있는지 확인용
    public bool InventoryIsOpen()
    {
        return inventoryUI != null && inventoryUI.activeSelf;
    }
}
