using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private List<QuestData> allQuestPool = new List<QuestData>();// ����Ʈ Ǯ
    [SerializeField] private QuestPopupUI popupUI;
    [SerializeField] private QuestPhone phone;
    [SerializeField] private QuestSlotUI questSlot; // �ִ� 3�� ����

    private List<QuestData> receivedQuests = new List<QuestData>(); //�̹� ������ ����Ʈ ���
    private List<QuestProgress> activeQuests = new List<QuestProgress>();


    private float questInterval = 10f; // ���� �ð� 2�� 120f
    private float timer;
    private QuestData pendingQuest;



    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= questInterval)
        {
            timer = 0f;
            TryGenerateQuest();
        }
    }

    void TryGenerateQuest()
    {
        if (pendingQuest == null && questSlot.HasEmptySlot)
        {
            pendingQuest = GetRandomQuest();

            if (pendingQuest != null)
            {
                Debug.Log($"[QuestManager] ����Ʈ ������: {pendingQuest.questTitle}");
                phone.ShowNotification();
            }
        }
    }

    public void OnQuestButtonClicked()
    {
        phone.HideNotification();

        if (pendingQuest != null)
        {
            popupUI.Show(pendingQuest);
        }
        else
        {
            popupUI.ShowNoQuest();      // ����Ʈ�� ������
        }
    }

    public void AcceptQuest()
{
    if (!questSlot.HasEmptySlot) return;

    QuestProgress newQuest = new QuestProgress(pendingQuest);

    questSlot.Assign(newQuest); 
    activeQuests.Add(newQuest);
    receivedQuests.Add(pendingQuest);

    pendingQuest = null;
    phone.HideNotification();
    popupUI.Hide();
}

    public void DeclineQuest()
    {
        if (pendingQuest != null)
        {
            receivedQuests.Add(pendingQuest); // ������ ����Ʈ�� ����
        }

        pendingQuest = null;
        phone.HideNotification();
        popupUI.Hide();
    }

    private QuestData GetRandomQuest()
    {
        List<QuestData> available = new List<QuestData>();

        foreach (var quest in allQuestPool)
        {
            if (!receivedQuests.Contains(quest))
            {
                available.Add(quest);
            }
        }

        if (available.Count == 0)
        {
            Debug.Log("[QuestManager] ���� ����Ʈ ����");
            return null;
        }

        Debug.Log($"[QuestManager] ����Ʈ Ǯ ����: {allQuestPool.Count}");
        return available[Random.Range(0, available.Count)];
    }

    public void ReportProgress(string targetName, int amount)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.quest.objectiveTarget == targetName)
            {
                quest.currentProgress += amount;
                Debug.Log($"[QuestManager] {targetName} ����Ʈ ���൵ ����: {quest.currentProgress}/{quest.quest.objectiveAmount}");

                if (quest.currentProgress >= quest.quest.objectiveAmount)
                {
                    Debug.Log($"[QuestManager] {targetName} ����Ʈ �Ϸ�!");
                    // TODO: ���⼭ ��� ����, ����Ʈ �Ϸ� ó�� �߰� ����
                }

                break; // ���� Ÿ�� ���� �� �����ϴ� �� ����
            }
        }
    }
}
