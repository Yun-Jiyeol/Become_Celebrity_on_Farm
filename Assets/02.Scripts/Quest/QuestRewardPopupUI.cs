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
        rewardTxt.text = $"보상: {quest.rewardGold}골드 / {quest.rewardExp} EXP";
        gameObject.SetActive(true);

        //보상 지급
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player != null)
        {
            player.AddGold(quest.rewardGold);
            // 경험치 시스템 추가
        }

        // 2초 후 자동 닫기
        Invoke(nameof(Hide), 2.5f);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}