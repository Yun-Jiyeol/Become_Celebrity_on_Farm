using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static QuestManager;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> slotTexts; // �ؽ�Ʈ ���Ե� (�ִ� 3��)

    private List<QuestProgress> acceptedQuests = new List<QuestProgress>();

    public bool HasEmptySlot => acceptedQuests.Count < slotTexts.Count;

    public void Assign(QuestData quest)
    {
        if (!HasEmptySlot)
        {
            Debug.LogWarning("����Ʈ ������ ���� á���ϴ�.");
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

                slotTexts[i].text = $"{i + 1}. {title}\n{description} ({current}/{target})\n���� �ð�: {time}";
            }
            else
            {
                slotTexts[i].text = "";
            }
        }
    }
    public class QuestProgress //// ����Ʈ ���� ����(�ð�)�� �����ϴ� Ŭ����
    {
        public QuestData quest;
        public float remainingTime;
        public int currentProgress = 0;

        public QuestProgress(QuestData quest)
        {
            this.quest = quest;
            remainingTime = quest.duration * 60f; // �� �� ��
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
            return $"{min:D2}�� {sec:D2}��";
        }
    }

    public void ClearAll()
    {
        acceptedQuests.Clear();
        UpdateUI();
    }
}