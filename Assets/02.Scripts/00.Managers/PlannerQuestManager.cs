using UnityEngine;

public class PlannerQuestManager : MonoBehaviour
{
    public static PlannerQuestManager Instance;

    [SerializeField] private GameObject dailyQuestUI;
    [SerializeField] private PlannerQuestData[] quests;

    private int lastReceivedDay = -1; // 마지막으로 퀘스트 받은 날짜
    private PlannerQuestData todayQuest;

    private bool isQuestAccepted = false;
    private bool isQuestCompleted = false;

    // 1일차 전용 조건들
    private bool didTill = false;
    private bool didPlant = false;
    private bool didWater = false;

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
        lastReceivedDay = TimeManager.Instance.currentDay;

        isQuestAccepted = true;
        isQuestCompleted = false;

        didTill = false;
        didPlant = false;
        didWater = false;
    }

    

    public void TryShowTodayQuest()
    {
        int today = TimeManager.Instance.currentDay;

        todayQuest = GetQuestForDay(today);

        OpenQuestUI(todayQuest);
    }

    private PlannerQuestData GetQuestForDay(int day)
    {
        int index = day % quests.Length;
        return quests[index];
    }

    private void OpenQuestUI(PlannerQuestData data)
    {
        dailyQuestUI.SetActive(true);

        // isAccepted = (오늘 날짜 == lastReceivedDay)
        bool isAccepted = (lastReceivedDay == TimeManager.Instance.currentDay);

        // UI 컨트롤러 불러와서 세팅
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
        }

        // 세 조건 모두 완료했는지 체크
        if (didTill && didPlant && didWater)
        {
            isQuestCompleted = true;
            Debug.Log("1일차 퀘스트 완료");

            GoldManager.Instance.AddGold(todayQuest.rewardGold);

            // 퀘스트 완료 UI 띄우기 해야함
        }
    }
}