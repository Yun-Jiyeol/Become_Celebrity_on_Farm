using UnityEngine;

public class PlannerQuestManager : MonoBehaviour
{
    public static PlannerQuestManager Instance;

    [SerializeField] private GameObject dailyQuestUI;
    [SerializeField] private PlannerQuestData[] quests;

    [SerializeField] private GameObject questRewardPopup;
    [SerializeField] private DailyQuestRewardPopupUI questRewardPopupUI;

    private int lastReceivedDay = -1; // 마지막으로 퀘스트 받은 날짜
    private PlannerQuestData todayQuest;

    private bool isQuestAccepted = false;
    private bool isQuestCompleted = false;

    private int copperCollected = 0;


    // 1일차 전용 조건들
    private bool didTill = false;
    private bool didPlant = false;
    private bool didWater = false;
    private bool didClean = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("PlannerQuestManager 초기화됨");
        }
        else
        {
            Destroy(gameObject);
        }

        if (dailyQuestUI == null)
            Debug.LogError("dailyQuestUI가 연결되지 않았습니다");
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

        Debug.Log("[PlannerQuestManager] 하루 지나서 퀘스트 상태 초기화됨");
    }

    

    public void TryShowTodayQuest()
    {
        int today = TimeManager.Instance.currentDay + 1;

        // 오늘 퀘스트가 이미 세팅된 상태면 다시 세팅하지 않음
        if (todayQuest == null || todayQuest.targetDay != today)
        {
            todayQuest = GetQuestForDay(today);
        }

        if (todayQuest == null)
        {
            Debug.LogWarning($"[PlannerQuestManager] {today}일차 퀘스트가 존재하지 않습니다.");
            return;
        }

        if (isQuestCompleted)
        {
            Debug.Log("이미 일일퀘스트를 완료했으므로 퀘스트 UI를 열지 않습니다.");
            return;
        }

        // 수락했는지 여부는 날짜로 판단
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

        Debug.LogWarning($"[PlannerQuest] {today}일차에 해당하는 퀘스트 없음!");
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

        // 행동 종류 체크
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
                Debug.Log("상점 방문 퀘스트 완료");
                GoldManager.Instance.AddGold(todayQuest.rewardGold);
                ShowQuestRewardUI();
                break;

            case "Clean":
                didClean = true;
                Debug.Log("didClean = true");

                // Clean만으로 완료되는 퀘스트 처리 (ex. 3일차)
                if (todayQuest.targetDay == 3)
                {
                    isQuestCompleted = true;
                    Debug.Log("3일차 퀘스트 완료 (Clean)");

                    GoldManager.Instance.AddGold(todayQuest.rewardGold);
                    ExpManager.Instance.AddExp(todayQuest.rewardExp);
                    ShowQuestRewardUI();
                    return;
                }

                break;

        }

        // 세 조건 모두 완료했는지 체크
        if (didTill && didPlant && didWater)
        {
            isQuestCompleted = true;
            Debug.Log("1일차 퀘스트 완료");

            ShowQuestRewardUI();
        }

        if (!isQuestAccepted || isQuestCompleted)
            return;   
    }
    /// <summary>
    /// 4일차 구리 채집 퀘스트
    public void ReportCopperCollected(int amount)
    {
        if (todayQuest != null && todayQuest.targetDay == 4)
        {
            copperCollected += amount;
            Debug.Log($"[일일퀘스트] 구리 채집: {copperCollected}/2");

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

        Debug.Log("일일 퀘스트 완료!");

        // 보상 지급
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
        Debug.Log("[PlannerQuestManager] ShowQuestRewardUI 호출됨");

        questRewardPopup.SetActive(true);
        questRewardPopupUI.SetReward(todayQuest.questTitle, todayQuest.rewardGold, todayQuest.rewardExp);
    }
    public void MarkQuestAsAccepted()
    {
        lastReceivedDay = TimeManager.Instance.currentDay;
        isQuestAccepted = true;
        isQuestCompleted = false;

        Debug.Log($"[PlannerQuestManager] 퀘스트 수락됨 - {lastReceivedDay}일차");
    }
}