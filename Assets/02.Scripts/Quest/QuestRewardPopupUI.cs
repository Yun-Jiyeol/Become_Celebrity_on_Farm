using UnityEngine;
using TMPro;

public class QuestRewardPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitleTxt;
    [SerializeField] private TextMeshProUGUI rewardTxt;

    private QuestData currentQuest;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show(QuestData quest)
    {
        currentQuest = quest;
        questTitleTxt.text = quest.questTitle;
        rewardTxt.text = $"����: {quest.rewardGold}��� / {quest.rewardExp} EXP";
        gameObject.SetActive(true);

        //���� ����
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player != null)
        {
            player.AddGold(quest.rewardGold);
            // ����ġ �ý��� �߰�
        }

        // 2�� �� �ڵ� �ݱ�
        Invoke(nameof(Hide), 2.5f);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}