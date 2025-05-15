using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DailyQuestRewardPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Button rewardButton;

    private bool rewardGiven = false;
    private int rewardGold;
    private int rewardExp;

    public void SetReward(string questTitle, int rewardGold, int rewardExp)
    {
        titleText.text = $"'{questTitle}' 퀘스트 완료!";
        rewardText.text = $"골드: {rewardGold}G\n경험치: {rewardExp} EXP";

        this.rewardGold = rewardGold;
        this.rewardExp = rewardExp;

        rewardGiven = false;
        rewardButton.interactable = true;
    }

    public void OnClickAcceptReward()
    {
        if (rewardGiven) return; // 중복 방지
        rewardGiven = true;

        //실제 보상 지급 처리
        GoldManager.Instance.AddGold(rewardGold);
        ExpManager.Instance.AddExp(rewardExp);

        Debug.Log($"보상 수령 완료! +{rewardGold}G, +{rewardExp}EXP");

        rewardButton.interactable = false;
        gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
