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

        questSlot.Assign(pendingQuest);
        receivedQuests.Add(pendingQuest); // 받은 퀘스트로 기록

        pendingQuest = null;
        phone.HideNotification();
        popupUI.Hide();
    }

    public void DeclineQuest()
    {
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

}
