using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlannerQuestUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI acceptButtonText;

    [SerializeField] private Button acceptButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        acceptButton.onClick.AddListener(OnClickAccept);
        closeButton.onClick.AddListener(OnClickClose);
    }

    public void SetQuest(PlannerQuestData data, bool isAccepted)
    {
        if (data == null)
        {
            Debug.LogError("PlannerQuestData�� null�Դϴ�.");
            return;
        }

        titleText.text = data.questTitle;
        descriptionText.text = data.description;

        if (isAccepted)
        {
            acceptButton.interactable = false;
            acceptButtonText.text = "���� �Ϸ�";
        }
        else
        {
            acceptButton.interactable = true;
            acceptButtonText.text = "�����ϱ�";
        }
    }

    private void OnClickAccept()
    {
        Debug.Log("[UI] ���� ��ư ����!");
        PlannerQuestManager.Instance.MarkQuestAcceptedToday();
        SetQuest(PlannerQuestManager.Instance.GetTodayQuestData(), true);
        // gameObject.SetActive(false);
    }

    private void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
