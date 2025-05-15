using UnityEngine;

[CreateAssetMenu(menuName = "Quest/PlannerQuest")]
public class PlannerQuestData : ScriptableObject
{
    public Season targetSeason;

    public int targetDayInSeason; // øπ: ∫Ω 1¿œ °Ê Spring, 1

    public string questTitle;

    [TextArea(2, 5)]
    public string description;

    public int rewardGold;

    public int rewardExp;

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
}
