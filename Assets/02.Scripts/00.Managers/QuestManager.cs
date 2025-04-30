using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private List<QuestData> allQuestPool = new List<QuestData>();// 퀘스트 풀
    [SerializeField] private QuestPopupUI popupUI;
    [SerializeField] private QuestPhone phone;
    [SerializeField] private QuestSlotUI questSlot; // 최대 3개 슬롯

    private List<QuestData> receivedQuests = new List<QuestData>(); //이미 수락한 퀘스트 목록
    private List<QuestProgress> activeQuests = new List<QuestProgress>();


    private float questInterval = 10f; // 실제 시간 2분 120f
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
                Debug.Log($"[QuestManager] 퀘스트 생성됨: {pendingQuest.questTitle}");
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
            popupUI.ShowNoQuest();      // 퀘스트가 없을때
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
            receivedQuests.Add(pendingQuest); // 거절한 퀘스트도 저장
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
            Debug.Log("[QuestManager] 남은 퀘스트 없음");
            return null;
        }

        Debug.Log($"[QuestManager] 퀘스트 풀 개수: {allQuestPool.Count}");
        return available[Random.Range(0, available.Count)];
    }

    public void ReportProgress(string targetName, int amount)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.quest.objectiveTarget == targetName)
            {
                quest.currentProgress += amount;
                Debug.Log($"[QuestManager] {targetName} 퀘스트 진행도 증가: {quest.currentProgress}/{quest.quest.objectiveAmount}");

                if (quest.currentProgress >= quest.quest.objectiveAmount)
                {
                    Debug.Log($"[QuestManager] {targetName} 퀘스트 완료!");
                    // TODO: 여기서 골드 지급, 퀘스트 완료 처리 추가 가능
                }

                break; // 같은 타겟 여러 개 증가하는 것 방지
            }
        }
    }
}
