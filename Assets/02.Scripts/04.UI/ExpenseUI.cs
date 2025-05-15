using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpenseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI goldText;

    Image expenseBg;
    Image expensePanel;

    Button nextButton;
    LoadingFader fader;
    Canvas endingCanvas;


    void Start()
    {
        nextButton = GetComponentInChildren<Button>();
        nextButton.onClick.AddListener(OnNextButtonClick);

        endingCanvas = GetComponentInParent<Canvas>();
        expenseBg = GetComponent<Image>();
        expensePanel = GetComponentInChildren<Image>();
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
            expenseBg.DOFade(0f, 0f);
            expenseBg.gameObject.SetActive(false);
            expensePanel.gameObject.SetActive(false);
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
        string month = TimeManager.Instance.currentMonth.ToString();
        string day = TimeManager.Instance.currentDay.ToString();
        dateText.text = $"{year}년 {month}월 {day}일";

        string add = "1000";
        string spend = "10000";
        string total = GoldManager.Instance.GetGold().ToString();
        goldText.text = $"{add}\n{spend}\n{total}";
    }
}
