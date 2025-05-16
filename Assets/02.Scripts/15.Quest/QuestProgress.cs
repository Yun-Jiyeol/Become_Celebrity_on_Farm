using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    public QuestData quest;
    public int currentProgress;
    public int startTime;       // ����Ʈ ���� �ð� (TimeManager.totalMinutes ����)
    public int durationMinutes; // ����Ʈ ��ȿ �ð� (�� ����)

    public QuestProgress(QuestData questData)
    {
        quest = questData;
        currentProgress = 0;

        startTime = TimeManager.Instance.totalMinutes;

        // 1�� = 144�� �� totalMinutes�� 10�� ������ ����
        durationMinutes = Mathf.RoundToInt(questData.duration * 144);
        Debug.Log($"[QuestProgress] ������ -> ���� �ð�: {startTime} / ��ȿ�ð�: {durationMinutes}��");
    }

    public int RemainingMinutes
    {
        get
        {
            int elapsed = TimeManager.Instance.totalMinutes - startTime;
            int remaining = Mathf.Max(0, durationMinutes - elapsed);
            Debug.Log($"[QuestProgress] �����ð� ��� -> total: {TimeManager.Instance.totalMinutes}, start: {startTime}, ����: {remaining}");
            return remaining;
        }
    }

    public string GetFormattedTime()
    {
        int minutes = RemainingMinutes;

        int days = minutes / 144;
        int hours = (minutes % 144) / 6; 
        return $"{days}�� {hours}�ð�";
    }

    public bool IsExpired => RemainingMinutes <= 0;
}