using UnityEngine;

[CreateAssetMenu(menuName = "Quest/PlannerQuest")]
public class PlannerQuestData : ScriptableObject
{
    public string questTitle;

    [TextArea(2, 5)]
    public string description;

    public int rewardGold;

    public int rewardExp;
}