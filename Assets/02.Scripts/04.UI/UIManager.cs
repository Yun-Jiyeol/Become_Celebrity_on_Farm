using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Prefabs")]
    public GameObject characterChoiceUIPrefab;

    [Header("Parent")]
    public Transform uiRoot; //캔버스 밑의 패널 등

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
}
