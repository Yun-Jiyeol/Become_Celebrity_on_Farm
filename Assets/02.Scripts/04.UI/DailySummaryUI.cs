using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DailySummaryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI goldText;

    [SerializeField] private Button nextButton;

    Image summaryBg;
    Image summaryPanel;

    LoadingFader fader;
    Canvas endingCanvas;


    void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);

        endingCanvas = GetComponentInParent<Canvas>();
        summaryBg = GetComponent<Image>();
        summaryPanel = GetComponentInChildren<Image>();
        fader = MapManager.Instance.fader;
    }

    void OnEnable()
    {
        SetText();
    }

    void OnNextButtonClick()
    {
        StartCoroutine(fader.Fade(() =>
        {
            summaryBg.DOFade(0f, 0f);
            summaryBg.gameObject.SetActive(false);
            summaryPanel.gameObject.SetActive(false);
            endingCanvas.gameObject.SetActive(false);

            MapManager.Instance.RefreshMap();
            TimeManager.Instance.AdvanceDay();
            TimeManager.Instance.currentHour = 6;
            TimeManager.Instance.currentMinute = 0;
            TimeManager.Instance.isSleeping = false;
        }));
    }

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
