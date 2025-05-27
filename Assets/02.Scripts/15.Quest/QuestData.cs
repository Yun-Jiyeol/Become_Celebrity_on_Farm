using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Popup Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questTitle;           // 퀘스트 제목
    [TextArea]
    public string questDescription;     // 퀘스트 설명
    public string shortDescription;     // 퀘스트 짧은 설명

    public int rewardGold;              // 보상 골드
    public int rewardExp;               // 보상 경험치

    public float duration = 20f;        // 게임 내 퀘스트 유지 시간 (단위: 분)

    public QuestType questType;
    public string objectiveTarget;      // 예: "닭 5마리 키우기", "브로콜리 수확"
    public int objectiveAmount;         // 목표 수치
    //public GameObject targetObject;   // 프리팹 등
    public Season.SeasonType availableSeason; //등장 가능한 계절
    public bool availableAllSeasons = false; //모든 계절 가능

    public enum QuestType
    {
        Farming,
        Fishing,
        Mining,
        Social,
        Delivery
    }
}
