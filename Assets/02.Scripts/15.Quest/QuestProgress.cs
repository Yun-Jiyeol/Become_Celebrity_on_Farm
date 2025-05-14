using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    public QuestData quest;       // � ����Ʈ�� ���� ������
    public int currentProgress;   // ���� �޼���
    public int remainingTicks; // �ΰ��� �ð� (�� ����)

    public QuestProgress(QuestData questData)
    {
        quest = questData;
        currentProgress = 0;
        remainingTicks = Mathf.RoundToInt(questData.duration * 144); // �Ϸ� 144ƽ = 24�ð� * 6 (10�� ����)
    }

    public void Tick()
    {
        remainingTicks = Mathf.Max(0, remainingTicks - 1);
    }

    public string GetFormattedTime()
    {
        int days = remainingTicks / 144;
        int hours = (remainingTicks % 144) / 6;
        return $"{days}�� {hours}�ð�";
    }
}