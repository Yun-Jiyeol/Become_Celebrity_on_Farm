using UnityEngine;

public class PlannerQuestManager : MonoBehaviour
{
    public static PlannerQuestManager Instance;

    [SerializeField] private GameObject dailyQuestUI;
    [SerializeField] private PlannerQuestData[] quests;

    [SerializeField] private GameObject questRewardPopup;
    [SerializeField] private DailyQuestRewardPopupUI questRewardPopupUI;

    [SerializeField] private GameObject questAlertIcon; //느낌표

    private int lastReceivedDay = -1; // 마지막으로 퀘스트 받은 날짜
    private PlannerQuestData todayQuest;

    private bool isQuestAccepted = false;
    private bool isQuestCompleted = false;


    // 1일차 전용 조건들
    private bool didTill = false;
    private bool didPlant = false;
    private bool didWater = false;
    private bool didClean = false;

    private int copperCollected = 0; // 4일차 구리 채집 퀘스트용
    private bool didFish = false; // 5일차 낚시 퀘스트용
    private bool didHarvest = false; // 6일차 농작물 수확 퀘스트용
    private bool didTalk = false; // 7일차 NPC 대화 퀘스트용


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
        Debug.Log("[일퀘] 하루 시작됨! 알림 표시 예정");

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
            Debug.Log("[일퀘] 느낌표 활성화 됨!");
        }
        else
        {
            Debug.LogWarning("questAlertIcon 연결 안 됨!");
        }

        Debug.Log("[PlannerQuestManager] 하루 지나서 퀘스트 상태 초기화됨");
    }

    

    public void TryShowTodayQuest()
    {
        int today = TimeManager.Instance.totalDaysPassed  + 1;

        todayQuest = GetQuestForDay(today);

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

        Debug.LogWarning($"[PlannerQuest] {day}일차에 해당하는 퀘스트 없음!");
        return null;

    }

    private void OpenQuestUI(PlannerQuestData data, bool isAccepted)
    {
        dailyQuestUI.SetActive(true);
        dailyQuestUI.GetComponent<PlannerQuestUIController>().SetQuest(data, isAccepted);

        if (questAlertIcon != null)
            questAlertIcon.SetActive(false); // 알림 숨기기
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

            case "Talk":

                if (todayQuest.targetDay == 7)
                {
                    didTalk = true;
                    isQuestCompleted = true;
                    Debug.Log("[PlannerQuest] 7일차 퀘스트 완료 (Talk)");

                    GoldManager.Instance.AddGold(todayQuest.rewardGold);
                    ExpManager.Instance.AddExp(todayQuest.rewardExp);
                    ShowQuestRewardUI();
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

    // 5일차 낚시 퀘스트
    public void ReportFishing()
    {
        if (todayQuest != null && todayQuest.targetDay == 5 && !isQuestCompleted && isQuestAccepted)
        {
            if (!didFish)
            {
                didFish = true;
                isQuestCompleted = true;

                Debug.Log("5일차 퀘스트 완료 (낚시)");

                GoldManager.Instance.AddGold(todayQuest.rewardGold);
                ExpManager.Instance.AddExp(todayQuest.rewardExp);
                ShowQuestRewardUI();
            }
        }
    }

    // 6일차 농작물 수확 퀘스트
    public void ReportHarvest()
    {
        if (todayQuest != null && todayQuest.targetDay == 6 && !isQuestCompleted && isQuestAccepted)
        {
            if (!didHarvest)
            {
                didHarvest = true;
                isQuestCompleted = true;
                Debug.Log("6일차 퀘스트 완료 (수확)");

                GoldManager.Instance.AddGold(todayQuest.rewardGold);
                ExpManager.Instance.AddExp(todayQuest.rewardExp);
                ShowQuestRewardUI();
            }
        }
    }

    private void ShowQuestRewardUI()
    {
        Debug.Log("[PlannerQuestManager] ShowQuestRewardUI 호출됨");

        questRewardPopup.SetActive(true);
        questRewardPopupUI.SetReward(todayQuest.questTitle, todayQuest.rewardGold, todayQuest.rewardExp);
    }
    public void MarkQuestAsAccepted()
    {
        lastReceivedDay = TimeManager.Instance.totalDaysPassed;
        isQuestAccepted = true;
        isQuestCompleted = false;

        Debug.Log($"[PlannerQuestManager] 퀘스트 수락됨 - {lastReceivedDay}일차");
    }
}