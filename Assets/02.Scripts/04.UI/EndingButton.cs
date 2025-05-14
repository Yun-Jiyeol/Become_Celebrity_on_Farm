using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EndingButton : MonoBehaviour
{
    [SerializeField] private Button endButton;
    [SerializeField] private Button nextDayButton;
    Canvas endingCanvas;
    LoadingFader fader;

    void Start()
    {
        endButton.onClick.AddListener(OnClickEndButton);
        nextDayButton.onClick.AddListener(OnNextDayButton);

        endingCanvas = GetComponentInParent<Canvas>();
        fader = MapManager.Instance.fader;
    }

    void OnClickEndButton()
    {
        //SceneManager.LoadScene("StartScene");
    }

    void OnNextDayButton()
    {
        StartCoroutine(fader.Fade(() =>
        {
            endingCanvas.gameObject.SetActive(false);
            MapManager.Instance.RefreshMap();
            TimeManager.Instance.AdvanceDay();
            TimeManager.Instance.currentHour = 6;
            TimeManager.Instance.currentMinute = 0;
            //if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.enabled = true;
        }));
    }
}
