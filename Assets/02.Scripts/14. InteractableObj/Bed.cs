using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
    [Header("Fader")]
    [SerializeField] private LoadingFader fader;

    [Header("UI")]
    [SerializeField] private Image endOfDaySelectUI;

    [Header("UI")]
    [SerializeField] private NextDay nextDay;

    [Header("Buttons")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;



    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYesButton);
        noButton.onClick.AddListener(OnClickNoButton);

        endOfDaySelectUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 침대 오른쪽 구석으로 충돌하면 선택 UI 활성화
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.enabled = false;
        endOfDaySelectUI.gameObject.SetActive(true);
    }

    void OnClickYesButton()
    {
        endOfDaySelectUI.gameObject.SetActive(false);
        nextDay.Sleep();
    }

    void OnClickNoButton()
    {
        endOfDaySelectUI.gameObject.SetActive(false);
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.enabled = true;
    }
}