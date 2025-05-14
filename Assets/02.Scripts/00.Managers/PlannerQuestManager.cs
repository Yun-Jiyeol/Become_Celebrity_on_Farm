using UnityEngine;

public class PlannerQuestManager : MonoBehaviour
{
    public static PlannerQuestManager Instance;

    [SerializeField] private GameObject dailyQuestUI;
    [SerializeField] private PlannerQuestData[] quests;

    private int lastReceivedDay = -1; // ���������� ����Ʈ ���� ��¥
    private PlannerQuestData todayQuest;

    private bool isQuestAccepted = false;
    private bool isQuestCompleted = false;

    // 1���� ���� ���ǵ�
    private bool didTill = false;
    private bool didPlant = false;
    private bool didWater = false;

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

        // isAccepted = (���� ��¥ == lastReceivedDay)
        bool isAccepted = (lastReceivedDay == TimeManager.Instance.currentDay);

        // UI ��Ʈ�ѷ� �ҷ��ͼ� ����
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
        }

        // �� ���� ��� �Ϸ��ߴ��� üũ
        if (didTill && didPlant && didWater)
        {
            isQuestCompleted = true;
            Debug.Log("1���� ����Ʈ �Ϸ�");

            GoldManager.Instance.AddGold(todayQuest.rewardGold);

            // ����Ʈ �Ϸ� UI ���� �ؾ���
        }
    }
}