using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterChoice : MonoBehaviour
{
    PlayerStats PlayerStats;

    public Button MaleBtn, FemaleBtn;
    public Button OKBtn, BackBtn;

    public TMP_InputField nameInputField, farmnameInputField;

    public CharacterAnimator characterAnimator;

    private string selectedCharacter = null;
    private bool isCharacterSelected = false;
    private void Start()
    {
        if (PlayerStats == null)
        {
            PlayerStats = FindObjectOfType<PlayerStats>();
            if (PlayerStats == null)
            {
                Debug.LogError("PlayerStats 컴포넌트를 찾을 수 없습니다.");
            }
        }
        Debug.Log("셋업!!!!"); //나중에 삭제
        Setup();
    }
    public void Setup()
    {
        Debug.Log("CharacterChoice.Setup() 호출됨"); //나중에 삭제
        MaleBtn.onClick.AddListener(() => SelectedCharacter("Male"));
        FemaleBtn.onClick.AddListener(() => SelectedCharacter("Female"));
        OKBtn.onClick.AddListener(OnOK);
        BackBtn.onClick.AddListener(OnBackBtn);

        selectedCharacter = null;
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

        PlayerStats.CharacterType = selectedCharacter;
        PlayerStats.Name = nameInputField.text;
        PlayerStats.FarmName = farmnameInputField.text;
        //값을 직접 받지 말고 변환시켜주는 메서드를 만들기.

        Debug.Log($"선택된 캐릭터: {PlayerStats.CharacterType}");
        Debug.Log($"이름: {PlayerStats.Name}");
        Debug.Log($"농장 이름: {PlayerStats.FarmName}");

        //게임씬 이동 로직 추가하기.
    }

    public void OnBackBtn()
    {
        Debug.Log("Back 버튼 클릭됨.");
        Destroy(gameObject);
    }
}
