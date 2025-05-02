using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
    [Header("Fader")]
    [SerializeField] private LoadingFader fader;

    [Header("UI")]
    [SerializeField] private Image endOfDaySelectUI;
    [SerializeField] private Canvas endingUI;

    [Header("Buttons")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;



    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYesButton);
        noButton.onClick.AddListener(OnClickNoButton);

        endOfDaySelectUI.gameObject.SetActive(false);
        endingUI.gameObject.SetActive(false);
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        // PlayerController?

        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = false;
        }

        endOfDaySelectUI.gameObject.SetActive(true);
    }


    void OnClickYesButton()
    {
        endOfDaySelectUI.gameObject.SetActive(false);

        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = true;
        }

        StartCoroutine(fader.Fade(
            () =>
            {
                endingUI.gameObject.SetActive(true);
            },

            () =>
            {
                // 다음 날 진행?
            }
            ));
    }

    void OnClickNoButton()
    {
        endOfDaySelectUI.gameObject.SetActive(false);

        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = true;
        }
    }
}
