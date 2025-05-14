using UnityEngine;
using UnityEngine.UI;

public class PlannerQuestUIController : MonoBehaviour
{
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        acceptButton.onClick.AddListener(OnClickAccept);
        closeButton.onClick.AddListener(OnClickClose);
    }

    private void OnClickAccept()
    {
        Debug.Log("����Ʈ ������!");
        PlannerQuestManager.Instance.MarkQuestAcceptedToday();
        gameObject.SetActive(false);
    }

    private void OnClickClose()
    {
        Debug.Log("����Ʈ â ����");
        gameObject.SetActive(false);
    }

    public void SetQuest(PlannerQuestData data, bool isAccepted)
    {
        // ����� ���� ǥ��
        transform.Find("TitleTxt").GetComponent<Text>().text = data.questTitle;
        transform.Find("DescriptionBtn/Text").GetComponent<Text>().text = data.description;

        // ��ư ���� ����
        acceptButton.interactable = !isAccepted;
    }
}
