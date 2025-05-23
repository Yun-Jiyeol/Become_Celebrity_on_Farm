using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class DailySummaryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI goldText;

    [SerializeField] private Button nextButton;

    Image summaryBg;
    Image summaryPanel;
    Canvas endingCanvas;
    NextDay nextDay;

    void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);

        endingCanvas = GetComponentInParent<Canvas>();
        summaryBg = GetComponent<Image>();
        summaryPanel = GetComponentInChildren<Image>();
        nextDay = FindObjectOfType<NextDay>();
    }

    void OnEnable()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.ReadyAudio["EndingSound"]);
        SetText();
    }

    void OnNextButtonClick()
    {
        StartCoroutine(GoToTheNextDay());
    }

    IEnumerator GoToTheNextDay()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["Button"]);

        MapManager.Instance.MoveMap(MapType.Home);

        yield return new WaitForSeconds(1f);

        summaryBg.DOFade(0f, 0f);
        summaryBg.gameObject.SetActive(false);
        summaryPanel.gameObject.SetActive(false);
        endingCanvas.gameObject.SetActive(false);

        // 랜덤 골드 50 ~ 100 손해
        int randomGold = Random.Range(5, 10) * 10;
        GoldManager.Instance.SpendGold(randomGold);

        if (!nextDay.isForced) TimeManager.Instance.AdvanceDay();

        TimeManager.Instance.currentHour = 6;
        TimeManager.Instance.currentMinute = 0;

        if (nextDay.isForced) nextDay.isForced = false;
        TimeManager.Instance.isSleeping = false;

        AudioManager.Instance.PlayBGM(AudioManager.Instance.ReadyAudio["MainBGM"]);
    }

    /// <summary>
    /// 정산 화면에 나올 골드 설정
    /// </summary>
    void SetText()
    {
        string year = TimeManager.Instance.currentYear.ToString();
        string month = (TimeManager.Instance.currentMonth + 1).ToString();
        string day = (TimeManager.Instance.currentDay + 1).ToString();
        dateText.text = $"{year}년 {month}월 {day}일";

        string add = $"{GoldManager.Instance.addAmount}G";
        string spend = $"{GoldManager.Instance.spendAmount}G";
        string total = $"{GoldManager.Instance.GetGold()}G";
        goldText.text = $"{add}\n{spend}\n{total}";
    }
}
