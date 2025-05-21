using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterChoice : UIBase
{
    [SerializeField] private bool femaleUnlocked = false; // 데모에서 여캐 선택 false

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
                Debug.LogError("PlayerStats 컴포넌트를 찾을 수 없습니다.");
            }
        }
        Debug.Log("셋업!!!!"); //나중에 삭제
        SelectedCharacter("Male");
        Setup();
    }
    public void Setup()
    {
        //Debug.Log("CharacterChoice.Setup() 호출됨"); //나중에 삭제
        //MaleBtn.onClick.AddListener(() => SelectedCharacter("Male"));
        //FemaleBtn.onClick.AddListener(() => SelectedCharacter("Female"));
        //OKBtn.onClick.AddListener(OnOK);
        //BackBtn.onClick.AddListener(OnBackBtn);

        //selectedCharacter = null;


        //데모 버전 여캐 잠금
        MaleBtn.onClick.AddListener(() => SelectedCharacter("Male"));
        OKBtn.onClick.AddListener(OnOK);
        BackBtn.onClick.AddListener(OnBackBtn);

        if (femaleUnlocked)
        {
            FemaleBtn.onClick.AddListener(() => SelectedCharacter("Female"));
            FemaleBtn.interactable = true;
            FemaleLock.SetActive(false); // 자물쇠 끄기
        }
        else
        {
            FemaleBtn.interactable = false;
            FemaleLock.SetActive(true); // 자물쇠 표시
        }
    }

    
    void SelectedCharacter(string character)
    {
        Debug.Log($"SelectedCharacter 호출됨: {character}"); //나중에 삭제
        selectedCharacter = character;
        isCharacterSelected = true;

        if (characterAnimator == null)
        {
            Debug.LogError("CharacterAnimator가 연결되지 않음.");
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
        Debug.Log($"선택 상태: {isCharacterSelected} / 캐릭터: {selectedCharacter}"); //나중에 삭제
        if (!isCharacterSelected)
        {
            Debug.Log("캐릭터를 선택하지 않음.");
            return;
        }

        if (string.IsNullOrEmpty(nameInputField.text) || string.IsNullOrEmpty(farmnameInputField.text))
        {
            Debug.Log("이름 또는 농장 이름을 입력하지 않음.");
            return;
        }

        PlayerStats.SetCharacterInfo(selectedCharacter, nameInputField.text, farmnameInputField.text);

        //게임씬 이동 로직 추가하기.
        AudioManager.Instance.StopBGM();
        SceneChangerManager.Instance.OnClick_LoadScene(SceneChangerManager.Instance.sceneNamesInBuild[2]);
    }

    public void OnBackBtn()
    {
        Debug.Log("Back 버튼 클릭됨.");
        AudioManager.Instance.StopBGM();
        SceneChangerManager.Instance.OnClick_LoadScene(SceneChangerManager.Instance.sceneNamesInBuild[0]);
    }
}
