using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestRewardPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitleTxt;
    [SerializeField] private TextMeshProUGUI rewardTxt;
    [SerializeField] private Button confirmButton;

    private QuestData currentQuest;

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnConfirm); // 버튼 클릭 시 보상 지급
        gameObject.SetActive(false);
    }

    public void Show(QuestData quest)
    {
        currentQuest = quest;
        questTitleTxt.text = quest.questTitle;
        rewardTxt.text = $"골드 : {quest.rewardGold} \n경험치 : {quest.rewardExp}";

        gameObject.SetActive(true);
    }

    private void OnConfirm()
    {
        if (currentQuest != null)
        {
            //보상 지급
            PlayerStats player = FindObjectOfType<PlayerStats>();
            if (player != null)
            {
                Debug.Log("[QuestRewardPopupUI] 골드 지급 시도");
                player.AddGold(currentQuest.rewardGold);
                // 경험치 시스템 추가
            }

            currentQuest = null;
        }

        gameObject.SetActive(false);
    }
}
