using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> slotTexts; // 텍스트 슬롯들 (최대 3개)

    private List<QuestProgress> acceptedQuests = new List<QuestProgress>();

    public bool HasEmptySlot => acceptedQuests.Count < slotTexts.Count;

    public void Assign(QuestProgress questProgress)
    {
        if (!HasEmptySlot)
        {
            Debug.LogWarning("퀘스트 슬롯이 가득 찼습니다.");
            return;
        }

        acceptedQuests.Add(questProgress);
        UpdateUI();
    }
    private void Update()
    {
        foreach (var q in acceptedQuests)
        {
            q.Update(Time.deltaTime);
        }

        UpdateUI();
    }
    private void UpdateUI()
    {
        for (int i = 0; i < slotTexts.Count; i++)
        {
            if (i < acceptedQuests.Count)
            {
                var q = acceptedQuests[i];
                string title = q.quest.questTitle;
                string description = q.quest.shortDescription;
                int current = q.currentProgress;
                int target = q.quest.objectiveAmount;
                string time = q.GetFormattedTime();

                slotTexts[i].text = $"{i + 1}. {title}\n{description} ({current}/{target})\n남은 시간: {time}";
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

    public void Remove(QuestProgress quest)
    {
        if (acceptedQuests.Contains(quest))
        {
            acceptedQuests.Remove(quest);
            UpdateUI();
            Debug.Log($"[QuestSlotUI] 퀘스트 제거됨: {quest.quest.questTitle}");
        }
        else
        {
            Debug.LogWarning("[QuestSlotUI] 제거 실패: 리스트에 없음");
        }
    }

    public void RemoveByTitle(string questTitle)
    {
        for (int i = 0; i < acceptedQuests.Count; i++)
        {
            if (acceptedQuests[i].quest.questTitle == questTitle)
            {
                acceptedQuests.RemoveAt(i);
                UpdateUI();
                break;
            }
        }
    }
}