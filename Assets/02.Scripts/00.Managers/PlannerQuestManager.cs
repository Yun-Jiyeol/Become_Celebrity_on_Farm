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

        if (today != lastReceivedDay)
        {
            lastReceivedDay = today;
            todayQuest = GetQuestForDay(today);
            OpenQuestUI(todayQuest);
        }
        else
        {
            Debug.Log("오늘 퀘스트는 이미 받았습니다.");
        }
    }

    private PlannerQuestData GetQuestForDay(int day)
    {
        int index = day % quests.Length;
        return quests[index];
    }

    private void OpenQuestUI(PlannerQuestData data)
    {
        dailyQuestUI.SetActive(true);

        // UI 요소 연결
        dailyQuestUI.transform.Find("TitleTxt").GetComponent<UnityEngine.UI.Text>().text = data.questTitle;
        dailyQuestUI.transform.Find("DescriptionBtn/Text").GetComponent<UnityEngine.UI.Text>().text = data.description;
    }
}