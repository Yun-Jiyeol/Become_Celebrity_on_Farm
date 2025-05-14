using UnityEngine;

[CreateAssetMenu(menuName = "Quest/PlannerQuest")]
public class PlannerQuestData : ScriptableObject
{
    public string questTitle;
    public string description;
    public int rewardGold;
    public int rewardExp;
}