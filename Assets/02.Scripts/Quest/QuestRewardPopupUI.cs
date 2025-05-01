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
        confirmButton.onClick.AddListener(OnConfirm); // ��ư Ŭ�� �� ���� ����
        gameObject.SetActive(false);
    }

    public void Show(QuestData quest)
    {
        currentQuest = quest;
        questTitleTxt.text = quest.questTitle;
        rewardTxt.text = $"��� : {quest.rewardGold} \n����ġ : {quest.rewardExp}";

        gameObject.SetActive(true);
    }

    private void OnConfirm()
    {
        if (currentQuest != null)
        {
            //���� ����
            PlayerStats player = FindObjectOfType<PlayerStats>();
            if (player != null)
            {
                Debug.Log("[QuestRewardPopupUI] ��� ���� �õ�");
                player.AddGold(currentQuest.rewardGold);
                // ����ġ �ý��� �߰�
            }

            currentQuest = null;
        }

        gameObject.SetActive(false);
    }
}
