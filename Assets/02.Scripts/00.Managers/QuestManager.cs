using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private List<QuestData> allQuestPool; // 퀘스트 풀
    [SerializeField] private QuestPopupUI popupUI;
    [SerializeField] private QuestPhone phone;
    [SerializeField] private List<QuestSlotUI> questSlots; // 최대 3개 슬롯

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
        if (pendingQuest == null)
        {
            pendingQuest = GetRandomQuest();
            phone.ShowNotification(); // 퀘스트 (폰) 알림
        }
    }

    public void OnQuestButtonClicked()
    {
        if (pendingQuest != null)
        {
            popupUI.Show(pendingQuest); // 수락/거절 UI 열기
        }
    }

    public void AcceptQuest()
    {
        foreach (var slot in questSlots)
        {
            if (!slot.HasQuest)
            {
                slot.Assign(pendingQuest);
                break;
            }
        }
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
        return allQuestPool[UnityEngine.Random.Range(0, allQuestPool.Count)];
    }
}
