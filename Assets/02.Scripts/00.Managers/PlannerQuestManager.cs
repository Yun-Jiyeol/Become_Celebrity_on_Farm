using UnityEngine;

public class PlannerQuestManager : MonoBehaviour
{
    public static PlannerQuestManager Instance;

    [SerializeField] private GameObject dailyQuestUI;
    [SerializeField] private PlannerQuestData[] quests;

    private int lastReceivedDay = -1; // 마지막으로 퀘스트 받은 날짜
    private PlannerQuestData todayQuest;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

    public void MarkQuestAcceptedToday()
    {
        lastReceivedDay = TimeManager.Instance.currentDay;
    }
}