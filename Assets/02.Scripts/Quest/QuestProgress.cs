using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    public QuestData quest;       // 어떤 퀘스트를 진행 중인지
    public int currentProgress;   // 현재 달성도
    public float remainingTime;   // 남은 시간 (초 단위)

    public QuestProgress(QuestData questData)
    {
        quest = questData;
        currentProgress = 0;
        remainingTime = questData.duration * 60f; // duration(분)을 초로 변환
    }

    public void Update(float deltaTime)
    {
        remainingTime -= deltaTime;
        remainingTime = Mathf.Max(0f, remainingTime); //음수 방지
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }
}