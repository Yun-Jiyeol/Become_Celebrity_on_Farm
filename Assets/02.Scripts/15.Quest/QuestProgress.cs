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

        // 1일 = 144분 → totalMinutes는 10분 단위로 증가
        durationMinutes = Mathf.RoundToInt(questData.duration * 144);
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

        int days = minutes / 144;
        int hours = (minutes % 144) / 6; 
        return $"{days}일 {hours}시간";
    }

    public bool IsExpired => RemainingMinutes <= 0;
}