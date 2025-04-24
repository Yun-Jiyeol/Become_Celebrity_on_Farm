using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    //[SerializeField] private Image questIcon;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private Button closeButton;

    private QuestData currentQuest;

    private void Awake()
    {
        acceptButton.onClick.AddListener(OnAccept);
        declineButton.onClick.AddListener(OnDecline);
        closeButton.onClick.AddListener(OnClose);

        Hide();
    }

    public void Show(QuestData quest)
    {
        currentQuest = quest;

        titleText.text = quest.questTitle;
        descriptionText.text = quest.questDescription;
        //questIcon.sprite = quest.questIcon;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnAccept()
    {
        QuestManager.Instance.AcceptQuest();
        Hide();
    }

    private void OnDecline()
    {
        QuestManager.Instance.DeclineQuest();
        Hide();
    }

    private void OnClose()
    {
        Hide();
    }
}