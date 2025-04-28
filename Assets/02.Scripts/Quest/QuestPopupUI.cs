using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private Button closeButton;

    private QuestData currentQuest;

    private void Awake()
    {
        acceptButton.onClick.AddListener(OnAccept);
        declineButton.onClick.AddListener(OnDecline);
        closeButton.onClick.AddListener(OnClose);

    }
    private void Start()
    {
        Hide();
    }

    public void Show(QuestData quest)
    {
        currentQuest = quest;

        titleText.text = quest.questTitle;
        descriptionText.text = quest.questDescription;

        acceptButton.gameObject.SetActive(true);
        declineButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        titleText.text = "";
        descriptionText.text = "";
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
        gameObject.SetActive(false);
    }

    public void ShowNoQuest()
    {
        titleText.text = "알림";
        descriptionText.text = "아직 퀘스트가 도착하지 않았습니다!";

        acceptButton.gameObject.SetActive(false);
        declineButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);

        gameObject.SetActive(true);
    }
}