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

        if (today != lastReceivedDay)
        {
            lastReceivedDay = today;
            todayQuest = GetQuestForDay(today);
            OpenQuestUI(todayQuest);
        }
        else
        {
            Debug.Log("���� ����Ʈ�� �̹� �޾ҽ��ϴ�.");
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

        // UI ��� ����
        dailyQuestUI.transform.Find("TitleTxt").GetComponent<UnityEngine.UI.Text>().text = data.questTitle;
        dailyQuestUI.transform.Find("DescriptionBtn/Text").GetComponent<UnityEngine.UI.Text>().text = data.description;
    }
}