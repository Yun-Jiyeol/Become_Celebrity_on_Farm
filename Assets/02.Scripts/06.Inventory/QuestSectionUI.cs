using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSectionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;
    [SerializeField] private TMP_Text questProgressText;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        List<QuestProgress> activeQuests = QuestManager.Instance.GetActiveQuests();

        if (activeQuests.Count == 0)
        {
            questTitleText.text = "진행 중인 퀘스트 없음";
            questDescriptionText.text = "-";
            questProgressText.text = "-";
            return;
        }

        QuestProgress quest = activeQuests[0]; // 가장 최근 퀘스트 하나만 보여줌

        questTitleText.text = quest.quest.questTitle;
        questDescriptionText.text = quest.quest.questDescription;
        questProgressText.text = $"{quest.currentProgress} / {quest.quest.objectiveAmount}";
    }
}

