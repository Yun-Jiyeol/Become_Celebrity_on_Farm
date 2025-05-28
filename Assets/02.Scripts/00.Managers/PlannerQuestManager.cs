using UnityEngine;

public class PlannerQuestManager : MonoBehaviour
{
    public static PlannerQuestManager Instance;

    [SerializeField] private GameObject dailyQuestUI;
    [SerializeField] private PlannerQuestData[] quests;

    [SerializeField] private GameObject questRewardPopup;
    [SerializeField] private DailyQuestRewardPopupUI questRewardPopupUI;

    [SerializeField] private GameObject questAlertIcon; //����ǥ

    private int lastReceivedDay = -1; // ���������� ����Ʈ ���� ��¥
    private PlannerQuestData todayQuest;

    private bool isQuestAccepted = false;
    private bool isQuestCompleted = false;


    // 1���� ���� ���ǵ�
    private bool didTill = false;
    private bool didPlant = false;
    private bool didWater = false;
    private bool didClean = false;

    private int copperCollected = 0; // 4���� ���� ä�� ����Ʈ��
    private bool didFish = false; // 5���� ���� ����Ʈ��
    private bool didHarvest = false; // 6���� ���۹� ��Ȯ ����Ʈ��
    private bool didTalk = false; // 7���� NPC ��ȭ ����Ʈ��


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
        Debug.Log("[����] �Ϸ� ���۵�! �˸� ǥ�� ����");

        lastReceivedDay = -1;  
        isQuestAccepted = false;    
        isQuestCompleted = false;

        didTill = false;
        didPlant = false;
        didWater = false;
        didClean = false;

        todayQuest = GetQuestForDay(TimeManager.Instance.totalDaysPassed + 1);

        if (questAlertIcon != null)
        {
            questAlertIcon.SetActive(true);
            Debug.Log("[����] ����ǥ Ȱ��ȭ ��!");
        }
        else
        {
            Debug.LogWarning("questAlertIcon ���� �� ��!");
        }

        Debug.Log("[PlannerQuestManager] �Ϸ� ������ ����Ʈ ���� �ʱ�ȭ��");
    }

    

    public void TryShowTodayQuest()
    {
        int today = TimeManager.Instance.totalDaysPassed  + 1;

        todayQuest = GetQuestForDay(today);

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
        bool isAccepted = (lastReceivedDay == TimeManager.Instance.totalDaysPassed);
        OpenQuestUI(todayQuest, isAccepted);
    }

    private PlannerQuestData GetQuestForDay(int day)
    {
        foreach (var quest in quests)
        {
            if (quest.targetDay == day)
                return quest;
        }

        Debug.LogWarning($"[PlannerQuest] {day}������ �ش��ϴ� ����Ʈ ����!");
        return null;

    }

    private void OpenQuestUI(PlannerQuestData data, bool isAccepted)
    {
        dailyQuestUI.SetActive(true);
        dailyQuestUI.GetComponent<PlannerQuestUIController>().SetQuest(data, isAccepted);

        if (questAlertIcon != null)
            questAlertIcon.SetActive(false); // �˸� �����
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

            case "Talk":

                if (todayQuest.targetDay == 7)
                {
                    didTalk = true;
                    isQuestCompleted = true;
                    Debug.Log("[PlannerQuest] 7���� ����Ʈ �Ϸ� (Talk)");

                    GoldManager.Instance.AddGold(todayQuest.rewardGold);
                    ExpManager.Instance.AddExp(todayQuest.rewardExp);
                    ShowQuestRewardUI();
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

    // 5���� ���� ����Ʈ
    public void ReportFishing()
    {
        if (todayQuest != null && todayQuest.targetDay == 5 && !isQuestCompleted && isQuestAccepted)
        {
            if (!didFish)
            {
                didFish = true;
                isQuestCompleted = true;

                Debug.Log("5���� ����Ʈ �Ϸ� (����)");

                GoldManager.Instance.AddGold(todayQuest.rewardGold);
                ExpManager.Instance.AddExp(todayQuest.rewardExp);
                ShowQuestRewardUI();
            }
        }
    }

    // 6���� ���۹� ��Ȯ ����Ʈ
    public void ReportHarvest()
    {
        if (todayQuest != null && todayQuest.targetDay == 6 && !isQuestCompleted && isQuestAccepted)
        {
            if (!didHarvest)
            {
                didHarvest = true;
                isQuestCompleted = true;
                Debug.Log("6���� ����Ʈ �Ϸ� (��Ȯ)");

                GoldManager.Instance.AddGold(todayQuest.rewardGold);
                ExpManager.Instance.AddExp(todayQuest.rewardExp);
                ShowQuestRewardUI();
            }
        }
    }

    private void ShowQuestRewardUI()
    {
        Debug.Log("[PlannerQuestManager] ShowQuestRewardUI ȣ���");

        questRewardPopup.SetActive(true);
        questRewardPopupUI.SetReward(todayQuest.questTitle, todayQuest.rewardGold, todayQuest.rewardExp);
    }
    public void MarkQuestAsAccepted()
    {
        lastReceivedDay = TimeManager.Instance.totalDaysPassed;
        isQuestAccepted = true;
        isQuestCompleted = false;

        Debug.Log($"[PlannerQuestManager] ����Ʈ ������ - {lastReceivedDay}����");
    }
}