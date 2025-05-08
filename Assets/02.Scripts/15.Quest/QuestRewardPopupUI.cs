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
            if (GoldManager.Instance != null)
            {
                Debug.Log("[QuestRewardPopupUI] ��� ���� �õ�");
                GoldManager.Instance.AddGold(currentQuest.rewardGold);
            }
            else
            {
                Debug.LogError("[QuestRewardPopupUI] GoldManager.Instance �� null�Դϴ�.");
            }

            currentQuest = null;
        }

        gameObject.SetActive(false);
    }
}
