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
        // �ΰ��� �ð� ��� ����Ʈ ����
        GenerateQuestBasedOnInGameTime();
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeChanged -= OnGameTick;
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
        Season.SeasonType currentSeason = TimeManager.Instance.season.CurrentSeason;

        List<QuestData> available = new List<QuestData>();

        foreach (var quest in allQuestPool)
        {
            bool isCorrectSeason = quest.availableAllSeasons || quest.availableSeason == currentSeason;

            if (!receivedQuests.Contains(quest) && isCorrectSeason)
            {
                available.Add(quest);
            }
        }

        return available.Count > 0 ? available[UnityEngine.Random.Range(0, available.Count)] : null;
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

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeChanged += OnGameTick;
        }
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeChanged -= OnGameTick;
        }
    }

    private void OnGameTick()
    {
        Debug.Log("[QuestManager] OnGameTick ȣ���");

        List<QuestProgress> expired = new List<QuestProgress>();

        foreach (var quest in activeQuests)
        {
            if (quest.IsExpired)
            {
                Debug.Log($"[QuestManager] ����Ʈ �����: {quest.quest.questTitle}");
                expired.Add(quest);
            }
        }

        foreach (var quest in expired)
        {
            questSlot.Remove(quest);
            activeQuests.Remove(quest);
        }

        questSlot.UpdateQuestUIManually(); // UI�� ��� �ݿ�
    }
    
}
