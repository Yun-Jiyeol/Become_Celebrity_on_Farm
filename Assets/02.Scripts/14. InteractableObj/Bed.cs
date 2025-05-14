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

    int preHour = 5;

    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYesButton);
        noButton.onClick.AddListener(OnClickNoButton);

        endOfDaySelectUI.gameObject.SetActive(false);
        endingUI.gameObject.SetActive(false);
    
        TimeManager.Instance.OnTimeChanged += CheckForcedSleepTime;
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
        Sleep();
    }

    void OnClickNoButton()
    {
        endOfDaySelectUI.gameObject.SetActive(false);
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.enabled = true;
    }

    /// <summary>
    /// 자정 확인용 함수
    /// </summary>
    void CheckForcedSleepTime()
    {
        int curHour = TimeManager.Instance.currentHour;

        // 24시가 된다면 강제 취침
        if (preHour == 23 && curHour == 6)
            ForcedSleep();

        preHour = curHour;
    }

    /// <summary>
    /// 다음날로 넘어감
    /// </summary>
    void Sleep()
    {
        StartCoroutine(fader.FadeOut(() =>
        {
            endingUI.gameObject.SetActive(true);
        }));
    }

    /// <summary>
    /// 자정이 될 때까지 안 자면 강제 취침
    /// </summary>
    void ForcedSleep()
    {
        // 방법
        // 1. 실행 중 TimeManager에서 22:50으로 바꿔서 preHour 23으로 세팅한 후
        // 2. 23:50으로 가서 자정 기다리면 강제 취침

        MapManager.Instance.MoveMap(MapType.Home);
        TimeManager.Instance.currentHour = 6;
        TimeManager.Instance.currentMinute = 0;

        // 골드 1000~2000 (temp. 랜덤) 손해
        int randomGold = Random.Range(10, 20) * 100;
        GoldManager.Instance.SpendGold(randomGold);
    }
}