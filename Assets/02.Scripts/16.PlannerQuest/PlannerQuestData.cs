using UnityEngine;

[CreateAssetMenu(menuName = "Quest/PlannerQuest")]
public class PlannerQuestData : ScriptableObject
{
    public string questTitle;

    [TextArea(2, 5)]
    public string description;

    public int targetDay; //ÀÌ Äù½ºÆ®°¡ ¶ß´Â ³¯Â¥

    public int rewardGold;

    public int rewardExp;

}