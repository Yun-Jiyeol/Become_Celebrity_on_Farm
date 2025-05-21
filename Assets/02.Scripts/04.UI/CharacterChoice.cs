using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterChoice : UIBase
{
    [SerializeField] private bool femaleUnlocked = false; // ���𿡼� ��ĳ ���� false

    PlayerStats PlayerStats;

    public Button MaleBtn, FemaleBtn;
    public Button OKBtn, BackBtn;

    public TMP_InputField nameInputField, farmnameInputField;

    public CharacterAnimator characterAnimator;

    public GameObject FemaleLock;

    private string selectedCharacter = null;
    private bool isCharacterSelected = false;
    private void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.ReadyAudio["SelectBGM"]);
        if (PlayerStats == null)
        {
            PlayerStats = SceneChangerManager.Instance.gameObject.GetComponent<PlayerStats>();
            if (PlayerStats == null)
            {
                Debug.LogError("PlayerStats ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        Debug.Log("�¾�!!!!"); //���߿� ����
        SelectedCharacter("Male");
        Setup();
    }
    public void Setup()
    {
        //Debug.Log("CharacterChoice.Setup() ȣ���"); //���߿� ����
        //MaleBtn.onClick.AddListener(() => SelectedCharacter("Male"));
        //FemaleBtn.onClick.AddListener(() => SelectedCharacter("Female"));
        //OKBtn.onClick.AddListener(OnOK);
        //BackBtn.onClick.AddListener(OnBackBtn);

        //selectedCharacter = null;


        //���� ���� ��ĳ ���
        MaleBtn.onClick.AddListener(() => SelectedCharacter("Male"));
        OKBtn.onClick.AddListener(OnOK);
        BackBtn.onClick.AddListener(OnBackBtn);

        if (femaleUnlocked)
        {
            FemaleBtn.onClick.AddListener(() => SelectedCharacter("Female"));
            FemaleBtn.interactable = true;
            FemaleLock.SetActive(false); // �ڹ��� ����
        }
        else
        {
            FemaleBtn.interactable = false;
            FemaleLock.SetActive(true); // �ڹ��� ǥ��
        }
    }

    
    void SelectedCharacter(string character)
    {
        Debug.Log($"SelectedCharacter ȣ���: {character}"); //���߿� ����
        selectedCharacter = character;
        isCharacterSelected = true;

        if (characterAnimator == null)
        {
            Debug.LogError("CharacterAnimator�� ������� ����.");
            return;
        }

        if (character == "Male")
        {
            characterAnimator.ShowAlex();
        }
        else if (character == "Female")
        {
            characterAnimator.ShowTori();
        }
    }

    public void OnOK()
    {
        Debug.Log($"���� ����: {isCharacterSelected} / ĳ����: {selectedCharacter}"); //���߿� ����
        if (!isCharacterSelected)
        {
            Debug.Log("ĳ���͸� �������� ����.");
            return;
        }

        if (string.IsNullOrEmpty(nameInputField.text) || string.IsNullOrEmpty(farmnameInputField.text))
        {
            Debug.Log("�̸� �Ǵ� ���� �̸��� �Է����� ����.");
            return;
        }

        PlayerStats.SetCharacterInfo(selectedCharacter, nameInputField.text, farmnameInputField.text);

        //���Ӿ� �̵� ���� �߰��ϱ�.
        AudioManager.Instance.StopBGM();
        SceneChangerManager.Instance.OnClick_LoadScene(SceneChangerManager.Instance.sceneNamesInBuild[2]);
    }

    public void OnBackBtn()
    {
        Debug.Log("Back ��ư Ŭ����.");
        AudioManager.Instance.StopBGM();
        SceneChangerManager.Instance.OnClick_LoadScene(SceneChangerManager.Instance.sceneNamesInBuild[0]);
    }
}
