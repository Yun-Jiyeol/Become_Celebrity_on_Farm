using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    public QuestData quest;
    public int currentProgress;
    public int startTime;       // 퀘스트 시작 시간 (TimeManager.totalMinutes 기준)
    public int durationMinutes; // 퀘스트 유효 시간 (분 단위)

    public QuestProgress(QuestData questData)
    {
        quest = questData;
        currentProgress = 0;

        startTime = TimeManager.Instance.totalMinutes;

        // duration은 인게임 분단위로 취급
        durationMinutes = Mathf.RoundToInt(questData.duration);
        Debug.Log($"[QuestProgress] 생성됨 -> 시작 시각: {startTime} / 유효시간: {durationMinutes}분");
    }

    public int RemainingMinutes
    {
        get
        {
            int elapsed = TimeManager.Instance.totalMinutes - startTime;
            int remaining = Mathf.Max(0, durationMinutes - elapsed);
            Debug.Log($"[QuestProgress] 남은시간 계산 -> total: {TimeManager.Instance.totalMinutes}, start: {startTime}, 남은: {remaining}");
            return remaining;
        }
    }

    public string GetFormattedTime()
    {
        int minutes = RemainingMinutes;

        minutes = (minutes / 10) * 10; // 10분 단위 내림

        int days = minutes / 1440; // 인게임 하루는 1440분
        int hours = (minutes % 1440) / 60;
        int mins = (minutes % 1440) % 60;

        return $"{days}일 {hours}시간 {mins}분";
    }

    public bool IsExpired => RemainingMinutes <= 0;
}