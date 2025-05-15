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
        titleText.text = $"'{questTitle}' ����Ʈ �Ϸ�!";
        rewardText.text = $"���: {rewardGold}G\n����ġ: {rewardExp} EXP";

        this.rewardGold = rewardGold;
        this.rewardExp = rewardExp;

        rewardGiven = false;
        rewardButton.interactable = true;
    }

    public void OnClickAcceptReward()
    {
        if (rewardGiven) return; // �ߺ� ����
        rewardGiven = true;

        //���� ���� ���� ó��
        GoldManager.Instance.AddGold(rewardGold);
        ExpManager.Instance.AddExp(rewardExp);

        Debug.Log($"���� ���� �Ϸ�! +{rewardGold}G, +{rewardExp}EXP");

        rewardButton.interactable = false;
        gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
