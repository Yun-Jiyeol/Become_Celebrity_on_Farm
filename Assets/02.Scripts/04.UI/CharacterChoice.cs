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
                Debug.LogError("PlayerStats ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        Debug.Log("�¾�!!!!"); //���߿� ����
        Setup();
    }
    public void Setup()
    {
        Debug.Log("CharacterChoice.Setup() ȣ���"); //���߿� ����
        MaleBtn.onClick.AddListener(() => SelectedCharacter("Male"));
        FemaleBtn.onClick.AddListener(() => SelectedCharacter("Female"));
        OKBtn.onClick.AddListener(OnOK);
        BackBtn.onClick.AddListener(OnBackBtn);

        selectedCharacter = null;
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

        PlayerStats.CharacterType = selectedCharacter;
        PlayerStats.Name = nameInputField.text;
        PlayerStats.FarmName = farmnameInputField.text;
        //���� ���� ���� ���� ��ȯ�����ִ� �޼��带 �����.

        Debug.Log($"���õ� ĳ����: {PlayerStats.CharacterType}");
        Debug.Log($"�̸�: {PlayerStats.Name}");
        Debug.Log($"���� �̸�: {PlayerStats.FarmName}");

        //���Ӿ� �̵� ���� �߰��ϱ�.
    }

    public void OnBackBtn()
    {
        Debug.Log("Back ��ư Ŭ����.");
        Destroy(gameObject);
    }
}
