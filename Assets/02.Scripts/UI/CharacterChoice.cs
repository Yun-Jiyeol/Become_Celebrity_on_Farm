using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChoice : MonoBehaviour
{
    public Button MaleBtn, FemaleBtn;
    public Button OKBtn, BackBtn;
    public Image Male, Female;

    private string selectedCharacter = null;
    public void Setup()
    {
        MaleBtn.onClick.AddListener(() => SelectedCharacter("Male"));
        FemaleBtn.onClick.AddListener(() => SelectedCharacter("Female"));
        OKBtn.onClick.AddListener(OnOK);
        BackBtn.onClick.AddListener(OnBackBtn);
    }

    void SelectedCharacter(string Character)
    {
        
    }

    void OnOK()
    {

    }
    void OnBackBtn()
    {
        
    }
}
