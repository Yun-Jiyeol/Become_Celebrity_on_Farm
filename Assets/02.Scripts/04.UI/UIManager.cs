using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Prefabs")]
    public GameObject characterChoiceUIPrefab;

    [Header("Parent")]
    public Transform uiRoot; //ĵ���� ���� �г� ��

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
}
