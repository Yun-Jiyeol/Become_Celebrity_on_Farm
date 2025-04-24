using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private List<QuestData> allQuestPool = new List<QuestData>();// 퀘스트 풀
    [SerializeField] private QuestPopupUI popupUI;
    [SerializeField] private QuestPhone phone;
    [SerializeField] private QuestSlotUI questSlot; // 최대 3개 슬롯

    private float questInterval = 120f; // 실제 시간 2분
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
            phone.ShowNotification();
        }
    }

    public void OnQuestButtonClicked()
    {
        if (pendingQuest != null)
        {
            popupUI.Show(pendingQuest); // 수락/거절 UI 열기
        }
        else
        {
            popupUI.ShowNoQuest(); // 퀘스트 없을 때 메시지
        }
    }

    public void AcceptQuest()
    {
        if (!questSlot.HasEmptySlot) return;

        questSlot.Assign(pendingQuest);
        pendingQuest = null;
        phone.HideNotification();
    }

    public void DeclineQuest()
    {
        pendingQuest = null;
        phone.HideNotification();
    }

    QuestData GetRandomQuest()
    {
        Debug.Log($"[QuestManager] 퀘스트 풀 개수: {allQuestPool.Count}");
        return allQuestPool[Random.Range(0, allQuestPool.Count)];
    }
}
