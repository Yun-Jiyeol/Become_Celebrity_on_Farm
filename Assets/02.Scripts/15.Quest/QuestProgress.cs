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

        // duration�� �ΰ��� �д����� ���
        durationMinutes = Mathf.RoundToInt(questData.duration);
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

        minutes = (minutes / 10) * 10; // 10�� ���� ����

        int days = minutes / 1440; // �ΰ��� �Ϸ�� 1440��
        int hours = (minutes % 1440) / 60;
        int mins = (minutes % 1440) % 60;

        return $"{days}�� {hours}�ð� {mins}��";
    }

    public bool IsExpired => RemainingMinutes <= 0;
}