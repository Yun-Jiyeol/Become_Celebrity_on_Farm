using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private List<QuestData> allQuestPool = new List<QuestData>();// ����Ʈ Ǯ
    [SerializeField] private QuestPopupUI popupUI;
    [SerializeField] private QuestPhone phone;
    [SerializeField] private QuestSlotUI questSlot; // �ִ� 3�� ����
    [SerializeField] private QuestRewardPopupUI questRewardPopupUI;


    private List<QuestData> receivedQuests = new List<QuestData>(); //�̹� ������ ����Ʈ ���
    private List<QuestProgress> activeQuests = new List<QuestProgress>();

    private QuestData pendingQuest;
    private int lastQuestTime = -1;



    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Update()
    {
        // �ð� ���� , ����Ʈ ���� ó��
        UpdateQuestTimers();

        // �ΰ��� �ð� ��� ����Ʈ ����
        GenerateQuestBasedOnInGameTime();
    }

    private void GenerateQuestBasedOnInGameTime()
    {
        int hour = TimeManager.Instance.currentHour;
        int minute = TimeManager.Instance.currentMinute;
        int totalMinutes = hour * 60 + minute;

        // ���� 8�� ~ 12�� ���̸� ����Ʈ ���� (480 ~ 720��)
        if (totalMinutes >= 480 && totalMinutes < 720)
        {
            if (totalMinutes % 20 == 0 && totalMinutes != lastQuestTime)
            {
                lastQuestTime = totalMinutes;

                Debug.Log($"[QuestManager] {hour}:{minute:D2} �� ����Ʈ üũ ��");

                // ���� ��� ����Ʈ ����
                if (pendingQuest != null)
                {
                    Debug.Log("[QuestManager] ���� ����Ʈ �ڵ� ���� ó���� (����/���� �� ��)");
                    receivedQuests.Add(pendingQuest); // ���� �ɷ� ó��
                    pendingQuest = null;
                    phone.HideNotification();
                    popupUI.Hide();
                }

                TryGenerateQuest();
            }
        }
    }
    public List<string> GetActiveQuestTargets()
    {
        List<string> targets = new List<string>();
        foreach (var quest in activeQuests)
        {
            targets.Add(quest.quest.objectiveTarget);
        }
        return targets;
    }
    private void UpdateQuestTimers()
    {
        List<QuestProgress> expired = new List<QuestProgress>();

        foreach (QuestProgress quest in activeQuests)
        {
            quest.Update(Time.deltaTime);

            if (quest.remainingTime <= 0f)
            {
                Debug.Log($"[QuestManager] ����Ʈ �����: {quest.quest.questTitle}");
                expired.Add(quest);
            }
        }

        foreach (QuestProgress quest in expired)
        {
            questSlot.Remove(quest);
            activeQuests.Remove(quest);
        }
    }

    void TryGenerateQuest()
    {
        if (pendingQuest == null && questSlot.HasEmptySlot)
        {
            pendingQuest = GetRandomQuest();

            if (pendingQuest != null)
            {
                Debug.Log($"[QuestManager] ����Ʈ ������: {pendingQuest.questTitle}");
                phone.ShowNotification();
            }
        }
    }

    public void OnQuestButtonClicked()
    {
        phone.HideNotification();

        if (pendingQuest != null)
        {
            popupUI.Show(pendingQuest);
        }
        else
        {
            popupUI.ShowNoQuest();      // ����Ʈ�� ������
        }
    }

    public void AcceptQuest()
    {
        if (!questSlot.HasEmptySlot) return;

        QuestProgress newQuest = new QuestProgress(pendingQuest);

        questSlot.Assign(newQuest); 
        activeQuests.Add(newQuest);
        receivedQuests.Add(pendingQuest); 

        pendingQuest = null;
        phone.HideNotification();
        popupUI.Hide();
    }

    public void DeclineQuest()
    {
        if (pendingQuest != null)
        {
            receivedQuests.Add(pendingQuest); // ������ ����Ʈ�� ����
        }

        pendingQuest = null;
        phone.HideNotification();
        popupUI.Hide();
    }

    private QuestData GetRandomQuest()
    {
        List<QuestData> available = new List<QuestData>();

        foreach (var quest in allQuestPool)
        {
            if (!receivedQuests.Contains(quest))
            {
                available.Add(quest);
            }
        }

        if (available.Count == 0)
        {
            Debug.Log("[QuestManager] ���� ����Ʈ ����");
            return null;
        }

        Debug.Log($"[QuestManager] ����Ʈ Ǯ ����: {allQuestPool.Count}");
        return available[Random.Range(0, available.Count)];
    }

    public void ReportProgress(string targetName, int amount)
    {
        QuestProgress completedQuest = null;

        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.quest.objectiveTarget == targetName)
            {
                quest.currentProgress += amount;
                Debug.Log($"[QuestManager] {targetName} ����Ʈ ���൵ ����: {quest.currentProgress}/{quest.quest.objectiveAmount}");

                if (quest.currentProgress >= quest.quest.objectiveAmount)
                {
                    Debug.Log($"[QuestManager] {targetName} ����Ʈ �Ϸ�!");
                    completedQuest = quest;

                    // ���Կ��� ����
                    questSlot.Remove(quest);

                    // ����Ʈ ��Ͽ����� ����
                    activeQuests.Remove(quest);

                }

                break; // ���� Ÿ�� ���� �� �����ϴ� �� ����
            }
        }
        if (completedQuest != null)
        {
            Debug.Log("[QuestManager] �Ϸ� �˾� ȣ�� �õ���");

            questSlot.Remove(completedQuest);
            activeQuests.Remove(completedQuest);

            //���� �˾� ȣ��
            questRewardPopupUI.Show(completedQuest.quest);
        }
    }

    public List<QuestProgress> GetActiveQuests()
    {
        return new List<QuestProgress>(activeQuests);
    }
}
