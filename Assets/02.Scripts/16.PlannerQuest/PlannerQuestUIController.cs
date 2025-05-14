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
        Debug.Log("퀘스트 수락됨!");
        PlannerQuestManager.Instance.MarkQuestAcceptedToday();
        gameObject.SetActive(false);
    }

    private void OnClickClose()
    {
        Debug.Log("퀘스트 창 닫힘");
        gameObject.SetActive(false);
    }

    public void SetQuest(PlannerQuestData data, bool isAccepted)
    {
        // 제목과 내용 표시
        transform.Find("TitleTxt").GetComponent<Text>().text = data.questTitle;
        transform.Find("DescriptionBtn/Text").GetComponent<Text>().text = data.description;

        // 버튼 상태 조절
        acceptButton.interactable = !isAccepted;
    }
}
