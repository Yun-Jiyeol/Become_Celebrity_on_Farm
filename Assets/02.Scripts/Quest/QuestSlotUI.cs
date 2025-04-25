using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> slotTexts; // 텍스트 슬롯들 (최대 3개)
    private List<QuestData> acceptedQuests = new List<QuestData>();

    public bool HasEmptySlot => acceptedQuests.Count < slotTexts.Count;

    public void Assign(QuestData quest)
    {
        if (!HasEmptySlot)
        {
            Debug.LogWarning("퀘스트 슬롯이 가득 찼습니다.");
            return;
        }

        acceptedQuests.Add(quest);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotTexts.Count; i++)
        {
            if (i < acceptedQuests.Count)
            {
                QuestData quest = acceptedQuests[i];
                string title = quest.questTitle;
                string description = quest.questDescription;
                string shortdescription = quest.shortDescription;
                int currentProgress = 0; // 초기값 (추후 업데이트로 변경 가능)
                int target = quest.objectiveAmount;

                slotTexts[i].text = $"{i + 1}.{title}\n{shortdescription} ({currentProgress}/{target})";
            }
            else
            {
                slotTexts[i].text = "";
            }
        }
    }

    public void ClearAll()
    {
        acceptedQuests.Clear();
        UpdateUI();
    }
}