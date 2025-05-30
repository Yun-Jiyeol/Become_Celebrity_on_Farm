using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    //[SerializeField] private QuestData[] allQuestPool; // 퀘스트 풀
    [SerializeField] private QuestPopupUI popupUI;
    [SerializeField] private QuestPhone phone;
    [SerializeField] private QuestSlotUI questSlot; // 최대 3개 슬롯
    [SerializeField] private QuestRewardPopupUI questRewardPopupUI;

    private List<QuestData> allQuestPool = new List<QuestData>();
    private List<QuestData> receivedQuests = new List<QuestData>(); //이미 수락한 퀘스트 목록
    private List<QuestProgress> activeQuests = new List<QuestProgress>();

    private QuestData pendingQuest;
    private int lastQuestTime = -1;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        LoadAllQuestsBySeason();

    }
    private void Update()
    {
        // 인게임 시간 기반 퀘스트 생성
        GenerateQuestBasedOnInGameTime();
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeChanged -= OnGameTick;
    }

    //퀘스트데이터 자동로드
    private void LoadAllQuestsBySeason()
    {
        allQuestPool.Clear();

        var all = Resources.LoadAll<QuestData>("Additional/QuestDatas/All");

        var currentSeason = TimeManager.Instance.season.CurrentSeason;

        switch (currentSeason)
        {
            case Season.SeasonType.Spring:
                allQuestPool.AddRange(Resources.LoadAll<QuestData>("Additional/QuestDatas/Spring"));
                break;
            case Season.SeasonType.Summer:
                allQuestPool.AddRange(Resources.LoadAll<QuestData>("Additional/QuestDatas/Summer"));
                break;
            case Season.SeasonType.Fall:
                allQuestPool.AddRange(Resources.LoadAll<QuestData>("Additional/QuestDatas/Fall"));
                break;
            case Season.SeasonType.Winter:
                allQuestPool.AddRange(Resources.LoadAll<QuestData>("Additional/QuestDatas/Winter"));
                break;
        }

        allQuestPool.AddRange(all);

        Debug.Log($"[PopupQuestManager] 계절 '{currentSeason}' 퀘스트 {allQuestPool.Count}개 로드됨 (All 포함)");
    }

    private void GenerateQuestBasedOnInGameTime()
    {
        int hour = TimeManager.Instance.currentHour;
        int minute = TimeManager.Instance.currentMinute;
        int totalMinutes = hour * 60 + minute;

        // 오전 8시 ~ 12시 사이만 퀘스트 생성 (480 ~ 720분)
        if (totalMinutes >= 480 && totalMinutes < 720)
        {
            if (totalMinutes % 20 == 0 && totalMinutes != lastQuestTime)
            {
                lastQuestTime = totalMinutes;

                Debug.Log($"[QuestManager] {hour}:{minute:D2} → 퀘스트 체크 중");

                // 기존 대기 퀘스트 제거
                if (pendingQuest != null)
                {
                    Debug.Log("[QuestManager] 이전 퀘스트 자동 만료 처리됨 (수락/거절 안 함)");
                    receivedQuests.Add(pendingQuest); // 받은 걸로 처리
                    pendingQuest = null;
                    phone.HideNotification();
                    popupUI.Hide();
                }

                TryGenerateQuest();
            }
        }
    }
    public List<string> GetActiveQuestTargets()
    {
        List<string> targets = new List<string>();
        foreach (var quest in activeQuests)
        {
            targets.Add(quest.quest.objectiveTarget);
        }
        return targets;
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
        if (allQuestPool.Count == 0) return null;
        return allQuestPool[Random.Range(0, allQuestPool.Count)];
    }

    public void ReportProgress(string targetName, int amount)
    {
        QuestProgress completedQuest = null;

        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.quest.objectiveTarget == targetName)
            {
                quest.currentProgress += amount;
                Debug.Log($"[QuestManager] {targetName} 퀘스트 진행도 증가: {quest.currentProgress}/{quest.quest.objectiveAmount}");

                if (quest.currentProgress >= quest.quest.objectiveAmount)
                {
                    Debug.Log($"[QuestManager] {targetName} 퀘스트 완료!");
                    completedQuest = quest;

                    // 슬롯에서 제거
                    questSlot.Remove(quest);

                    // 퀘스트 목록에서도 제거
                    activeQuests.Remove(quest);

                }

                break; // 같은 타겟 여러 개 증가하는 것 방지
            }
        }
        if (completedQuest != null)
        {
            Debug.Log("[QuestManager] 완료 팝업 호출 시도됨");

            questSlot.Remove(completedQuest);
            activeQuests.Remove(completedQuest);

            //보상 팝업 호출
            questRewardPopupUI.Show(completedQuest.quest);
        }
    }

    public List<QuestProgress> GetActiveQuests()
    {
        return new List<QuestProgress>(activeQuests);
    }

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeChanged += OnGameTick;
        }
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeChanged -= OnGameTick;
        }
    }

    private void OnGameTick()
    {
        Debug.Log("[QuestManager] OnGameTick 호출됨");

        List<QuestProgress> expired = new List<QuestProgress>();

        foreach (var quest in activeQuests)
        {
            if (quest.IsExpired)
            {
                Debug.Log($"[QuestManager] 퀘스트 만료됨: {quest.quest.questTitle}");
                expired.Add(quest);
            }
        }

        foreach (var quest in expired)
        {
            questSlot.Remove(quest);
            activeQuests.Remove(quest);
        }

        questSlot.UpdateQuestUIManually(); // UI도 즉시 반영
    }
    
}
