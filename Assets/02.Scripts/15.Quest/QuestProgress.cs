using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    public QuestData quest;       // 어떤 퀘스트를 진행 중인지
    public int currentProgress;   // 현재 달성도
    public int remainingTicks; // 인게임 시간 (분 단위)

    public QuestProgress(QuestData questData)
    {
        quest = questData;
        currentProgress = 0;
        remainingTicks = Mathf.RoundToInt(questData.duration * 144); // 하루 144틱 = 24시간 * 6 (10분 단위)
    }

    public void Tick()
    {
        remainingTicks = Mathf.Max(0, remainingTicks - 1);
    }

    public string GetFormattedTime()
    {
        int days = remainingTicks / 144;
        int hours = (remainingTicks % 144) / 6;
        return $"{days}일 {hours}시간";
    }
}