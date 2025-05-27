using UnityEngine;

public class PlannerQuestManager : MonoBehaviour
{
    public static PlannerQuestManager Instance;

    [SerializeField] private GameObject dailyQuestUI;
    [SerializeField] private PlannerQuestData[] quests;

    [SerializeField] private GameObject questRewardPopup;
    [SerializeField] private DailyQuestRewardPopupUI questRewardPopupUI;

    private int lastReceivedDay = -1; // ���������� ����Ʈ ���� ��¥
    private PlannerQuestData todayQuest;

    private bool isQuestAccepted = false;
    private bool isQuestCompleted = false;

    private int copperCollected = 0;


    // 1���� ���� ���ǵ�
    private bool didTill = false;
    private bool didPlant = false;
    private bool didWater = false;
    private bool didClean = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("PlannerQuestManager �ʱ�ȭ��");
        }
        else
        {
            Destroy(gameObject);
        }

        if (dailyQuestUI == null)
            Debug.LogError("dailyQuestUI�� ������� �ʾҽ��ϴ�");
    }

    public void MarkQuestAcceptedToday()
    {
        lastReceivedDay = -1;  
        isQuestAccepted = false;    
        isQuestCompleted = false;

        didTill = false;
        didPlant = false;
        didWater = false;
        didClean = false;

        todayQuest = GetQuestForDay(TimeManager.Instance.currentDay + 1);

        Debug.Log("[PlannerQuestManager] �Ϸ� ������ ����Ʈ ���� �ʱ�ȭ��");
    }

    

    public void TryShowTodayQuest()
    {
        int today = TimeManager.Instance.currentDay + 1;

        // ���� ����Ʈ�� �̹� ���õ� ���¸� �ٽ� �������� ����
        if (todayQuest == null || todayQuest.targetDay != today)
        {
            todayQuest = GetQuestForDay(today);
        }

        if (todayQuest == null)
        {
            Debug.LogWarning($"[PlannerQuestManager] {today}���� ����Ʈ�� �������� �ʽ��ϴ�.");
            return;
        }

        if (isQuestCompleted)
        {
            Debug.Log("�̹� ��������Ʈ�� �Ϸ������Ƿ� ����Ʈ UI�� ���� �ʽ��ϴ�.");
            return;
        }

        // �����ߴ��� ���δ� ��¥�� �Ǵ�
        bool isAccepted = (lastReceivedDay == TimeManager.Instance.currentDay);
        OpenQuestUI(todayQuest, isAccepted);
    }

    private PlannerQuestData GetQuestForDay(int day)
    {
        int today = TimeManager.Instance.currentDay + 1; 

        foreach (var quest in quests)
        {
            if (quest.targetDay == today)
                return quest;
        }

        Debug.LogWarning($"[PlannerQuest] {today}������ �ش��ϴ� ����Ʈ ����!");
        return null;

    }

    private void OpenQuestUI(PlannerQuestData data, bool isAccepted)
    {
        dailyQuestUI.SetActive(true);
        dailyQuestUI.GetComponent<PlannerQuestUIController>().SetQuest(data, isAccepted);
    }

    public PlannerQuestData GetTodayQuestData()
    {
        return todayQuest;
    }
    public void ReportAction(string actionType)
    {
        if (!isQuestAccepted || isQuestCompleted)
            return;

        // �ൿ ���� üũ
        switch (actionType)
        {
            case "Till":
                didTill = true;
                break;
            case "Plant":
                didPlant = true;
                break;
            case "Water":
                didWater = true;
                break;

            case "VisitShop":
                isQuestCompleted = true;
                Debug.Log("���� �湮 ����Ʈ �Ϸ�");
                GoldManager.Instance.AddGold(todayQuest.rewardGold);
                ShowQuestRewardUI();
                break;

            case "Clean":
                didClean = true;
                Debug.Log("didClean = true");

                // Clean������ �Ϸ�Ǵ� ����Ʈ ó�� (ex. 3����)
                if (todayQuest.targetDay == 3)
                {
                    isQuestCompleted = true;
                    Debug.Log("3���� ����Ʈ �Ϸ� (Clean)");

                    GoldManager.Instance.AddGold(todayQuest.rewardGold);
                    ExpManager.Instance.AddExp(todayQuest.rewardExp);
                    ShowQuestRewardUI();
                    return;
                }

                break;

        }

        // �� ���� ��� �Ϸ��ߴ��� üũ
        if (didTill && didPlant && didWater)
        {
            isQuestCompleted = true;
            Debug.Log("1���� ����Ʈ �Ϸ�");

            ShowQuestRewardUI();
        }

        if (!isQuestAccepted || isQuestCompleted)
            return;   
    }
    /// <summary>
    /// 4���� ���� ä�� ����Ʈ
    public void ReportCopperCollected(int amount)
    {
        if (todayQuest != null && todayQuest.targetDay == 4)
        {
            copperCollected += amount;
            Debug.Log($"[��������Ʈ] ���� ä��: {copperCollected}/2");

            if (copperCollected >= 2)
            {
                CheckQuestCompletion();
            }
        }
    }
    private void CheckQuestCompletion()
    {
        if (isQuestCompleted) return;

        isQuestCompleted = true;

        Debug.Log("���� ����Ʈ �Ϸ�!");

        // ���� ����
        if (todayQuest != null)
        {
            GoldManager.Instance.AddGold(todayQuest.rewardGold);
            ExpManager.Instance.AddExp(todayQuest.rewardExp);
        }

        ShowQuestRewardUI();
    }
    /// </summary>

    private void ShowQuestRewardUI()
    {
        Debug.Log("[PlannerQuestManager] ShowQuestRewardUI ȣ���");

        questRewardPopup.SetActive(true);
        questRewardPopupUI.SetReward(todayQuest.questTitle, todayQuest.rewardGold, todayQuest.rewardExp);
    }
    public void MarkQuestAsAccepted()
    {
        lastReceivedDay = TimeManager.Instance.currentDay;
        isQuestAccepted = true;
        isQuestCompleted = false;

        Debug.Log($"[PlannerQuestManager] ����Ʈ ������ - {lastReceivedDay}����");
    }
}