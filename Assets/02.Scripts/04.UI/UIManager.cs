using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Prefabs")]
    public GameObject characterChoiceUIPrefab;

    [Header("Parent")]
    public Transform uiRoot; //ĵ���� ���� �г� ��

    [Header("Ingame UI")]
    public GameObject inventoryUI; // <- �κ��丮 UI ����

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("�ټ��� UIManager ����!");
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

    // �κ��丮 ���� �ݱ� Toggle
    public void ToggleInventoryUI()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("�κ��丮 UI�� ���� �ȵ�");
            return;
        }

        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive);
    }

    // �κ��丮 �����ִ��� Ȯ�ο�
    public bool InventoryIsOpen()
    {
        return inventoryUI != null && inventoryUI.activeSelf;
    }
}
