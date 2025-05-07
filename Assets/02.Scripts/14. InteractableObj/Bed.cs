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

        Debug.Log($"[Bed] pre: {preHour} cur: {curHour}");
    }

    /// <summary>
    /// 다음날로 넘어감
    /// </summary>
    void Sleep()
    {
        //TimeManager.Instance.isSleeping = true;
        MapManager.Instance.ReloadMap();
    }

    /// <summary>
    /// 자정이 될 때까지 안 자면 강제 취침
    /// </summary>
    void ForcedSleep()
    {
        MapManager.Instance.MoveMap(MapType.Home);
        TimeManager.Instance.currentHour = 6;
        TimeManager.Instance.currentMinute = 0;

        // 골드 1000~2000 (temp) 손해
        int randomGold = Random.Range(10, 20) * 100;
        GoldManager.Instance.SpendGold(randomGold);
    }
}