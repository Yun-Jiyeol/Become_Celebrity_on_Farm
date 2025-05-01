using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    public QuestData quest;       // � ����Ʈ�� ���� ������
    public int currentProgress;   // ���� �޼���
    public float remainingTime;   // ���� �ð� (�� ����)

    public QuestProgress(QuestData questData)
    {
        quest = questData;
        currentProgress = 0;
        remainingTime = questData.duration * 60f; // duration(��)�� �ʷ� ��ȯ
    }

    public void Update(float deltaTime)
    {
        remainingTime -= deltaTime;
        remainingTime = Mathf.Max(0f, remainingTime); //���� ����
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }
}