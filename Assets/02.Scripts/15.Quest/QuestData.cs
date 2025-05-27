using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Popup Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questTitle;           // ����Ʈ ����
    [TextArea]
    public string questDescription;     // ����Ʈ ����
    public string shortDescription;     // ����Ʈ ª�� ����

    public int rewardGold;              // ���� ���
    public int rewardExp;               // ���� ����ġ

    public float duration = 20f;        // ���� �� ����Ʈ ���� �ð� (����: ��)

    public QuestType questType;
    public string objectiveTarget;      // ��: "�� 5���� Ű���", "����ݸ� ��Ȯ"
    public int objectiveAmount;         // ��ǥ ��ġ
    //public GameObject targetObject;   // ������ ��
    public Season.SeasonType availableSeason; //���� ������ ����
    public bool availableAllSeasons = false; //��� ���� ����

    public enum QuestType
    {
        Farming,
        Fishing,
        Mining,
        Social,
        Delivery
    }
}
