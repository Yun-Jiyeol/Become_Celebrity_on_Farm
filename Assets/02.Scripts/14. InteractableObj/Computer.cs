using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Computer : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private Image dailyQuestUI;

    [Header("Button")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button closeButton;


    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYesButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        dailyQuestUI.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ?
        dailyQuestUI.gameObject.SetActive(true);
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = false;
        }
        Debug.Log("Click");
    }

    void OnClickYesButton()
    {
        // 퀘스트 받음

        dailyQuestUI.gameObject.SetActive(false);
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = true;
        }
    }


    void OnClickCloseButton()
    {
        dailyQuestUI.gameObject.SetActive(false);

        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = true;
        }
    }
}
