using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OnClickTutorialBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Button tutorialBtn;
    TutorialUI tutorialUI; 

    void Start()
    {
        tutorialBtn = GetComponentInChildren<Button>();
        tutorialUI = GetComponentInChildren<TutorialUI>();
        tutorialBtn.onClick.AddListener(OnClickBtn);

        tutorialUI.gameObject.SetActive(false);
    }

    void OnClickBtn()
    {
        tutorialUI.gameObject.SetActive(!tutorialUI.gameObject.activeSelf);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["GetItem"]);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.player == null) return;
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.actions["Click"].Disable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.player == null) return;
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.actions["Click"].Enable();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.player == null) return;
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.enabled = false;
    }
}
