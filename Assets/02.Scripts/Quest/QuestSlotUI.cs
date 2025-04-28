using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static QuestManager;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> slotTexts; // 텍스트 슬롯들 (최대 3개)

    private List<QuestProgress> acceptedQuests = new List<QuestProgress>();

    public bool HasEmptySlot => acceptedQuests.Count < slotTexts.Count;

    public void Assign(QuestData quest)
    {
        if (!HasEmptySlot)
        {
            Debug.LogWarning("퀘스트 슬롯이 가득 찼습니다.");
            return;
        }

        acceptedQuests.Add(new QuestProgress(quest));
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
    public class QuestProgress //// 퀘스트 진행 상태(시간)를 관리하는 클래스
    {
        public QuestData quest;
        public float remainingTime;
        public int currentProgress = 0;

        public QuestProgress(QuestData quest)
        {
            this.quest = quest;
            remainingTime = quest.duration * 60f; // 분 → 초
        }

        public void Update(float deltaTime)
        {
            remainingTime -= deltaTime;
            remainingTime = Mathf.Max(remainingTime, 0f);
        }

        public string GetFormattedTime()
        {
            int min = Mathf.FloorToInt(remainingTime / 60f);
            int sec = Mathf.FloorToInt(remainingTime % 60f);
            return $"{min:D2}분 {sec:D2}초";
        }
    }

    public void ClearAll()
    {
        acceptedQuests.Clear();
        UpdateUI();
    }
}