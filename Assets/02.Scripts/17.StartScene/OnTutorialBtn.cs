using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnTutorialBtn : StartSceneBtn
{
    [SerializeField] private Image tutorialUI;

    void Start()
    {
        tutorialUI.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        tutorialUI.gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["GetItem"]);
    }
}
