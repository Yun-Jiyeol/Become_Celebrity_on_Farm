using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : UIBase
{
    public Button QuestBtn;
    public Button CloseBtn;
    public GameObject Quest;
    public void SetUp()
    {
        //setup()�ߺ� ����
        QuestBtn.onClick.RemoveAllListeners();
        CloseBtn.onClick.RemoveAllListeners();

        QuestBtn.onClick.AddListener(OnQuestBtn);
        CloseBtn.onClick.AddListener(OnCloseBtn);

        Quest.gameObject.SetActive(false);
        QuestBtn.gameObject.SetActive(true);
        CloseBtn.gameObject.SetActive(true);
    }

    public void Awake()
    {
        SetUp();
    }

    public override void Show()
    {

        Quest?.SetActive(true);
        QuestBtn.gameObject.SetActive(false);
    }

    public override void Hide()
    {

        Quest?.SetActive(false);
        QuestBtn.gameObject.SetActive(true);

    }
    public void OnQuestBtn()
    {
        Debug.Log("����Ʈ ��ư Ŭ����");
        Show();
    }
    public void OnCloseBtn()
    {
        Debug.Log("����Ʈ �ݱ� ��ư Ŭ����");
        Hide();
    }
}
