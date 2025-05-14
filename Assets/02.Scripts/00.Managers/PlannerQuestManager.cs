using UnityEngine;

public class PlannerQuestManager : MonoBehaviour
{
    public static PlannerQuestManager Instance;

    [SerializeField] private GameObject dailyQuestUI;
    [SerializeField] private PlannerQuestData[] quests;

    private int lastReceivedDay = -1; // ���������� ����Ʈ ���� ��¥
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

        // isAccepted = (���� ��¥ == lastReceivedDay)
        bool isAccepted = (lastReceivedDay == TimeManager.Instance.currentDay);

        // UI ��Ʈ�ѷ� �ҷ��ͼ� ����
        dailyQuestUI.GetComponent<PlannerQuestUIController>().SetQuest(data, isAccepted);
    }

    public void MarkQuestAcceptedToday()
    {
        lastReceivedDay = TimeManager.Instance.currentDay;
    }
}