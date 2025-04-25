using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> slotTexts; // �ؽ�Ʈ ���Ե� (�ִ� 3��)
    private List<QuestData> acceptedQuests = new List<QuestData>();

    public bool HasEmptySlot => acceptedQuests.Count < slotTexts.Count;

    public void Assign(QuestData quest)
    {
        if (!HasEmptySlot)
        {
            Debug.LogWarning("����Ʈ ������ ���� á���ϴ�.");
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
                int currentProgress = 0; // �ʱⰪ (���� ������Ʈ�� ���� ����)
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