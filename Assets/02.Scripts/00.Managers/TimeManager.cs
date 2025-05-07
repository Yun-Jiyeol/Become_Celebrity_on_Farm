using TMPro;
using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public TMP_Text timeText;   // TimeTxt �ؽ�Ʈ
    public TMP_Text dayText;    // DayTxt �ؽ�Ʈ

    [Header("���� �ð� ���� ���� (�׽�Ʈ��)")]
    [Range(0, 23)] public int currentHour = 6;
    [Range(0, 59)] public int currentMinute = 0;
    [Range(0, 365)] public int currentDay = 0; // 0 = ������

    private readonly string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    private readonly string[] seasons = { "��", "����", "����", "�ܿ�" };

    public string CurrentWeekday => weekdays[currentDay % 7];
    public string CurrentSeason => seasons[(currentDay / 28) % 4];  // ���� ���

    private bool isSleeping = false;
    private bool isFainted = false;

    [Header("Time Settings")]
    [Tooltip("���� �� �ʸ��� ���� �ð� 10���� �带�� ����")]
    public float realSecondsPerGameTenMinutes = 10f;

    private float timeTick => realSecondsPerGameTenMinutes;

    private Season season;

    [Header("���� ���� ���� (TimeManager �� Season)")]
    public bool overrideSeasonInInspector = false;
    public Season.SeasonType manualSeason;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inspector���� ������� �ʾ��� ��� �ڵ����� ã��
        if (timeText == null)
        {
            var timeTransform = transform.root.Find("Canvases/InGameCanvas/PlayerInGameUI/ClockGold/TimeTxt");
            if (timeTransform != null)
                timeText = timeTransform.GetComponent<TMP_Text>();
            else
                Debug.LogWarning("TimeTxt�� ã�� �� �����ϴ�. Inspector�� ���� ������ �ּ���.");
        }

        if (dayText == null)
        {
            var dayTransform = transform.root.Find("Canvases/InGameCanvas/PlayerInGameUI/ClockGold/DayTxt");
            if (dayTransform != null)
                dayText = dayTransform.GetComponent<TMP_Text>();
            else
                Debug.LogWarning("DayTxt�� ã�� �� �����ϴ�. Inspector�� ���� ������ �ּ���.");
        }

        season = FindObjectOfType<Season>();
    }

    private void Start()
    {
        currentHour = 6;
        currentMinute = 0;

        if (season != null)
        {
            season.SetCurrentDay(currentDay);

            if (overrideSeasonInInspector)
            {
                season.SetCurrentSeason(manualSeason);
            }
        }

        // ���� �� ù ��° Ÿ���÷ο� ���� ���� �ʱ� �ð��� ǥ���ϵ��� ó��
        timeText.text = GetFormattedTime();
        dayText.text = GetFormattedDay();  // ����, ����, ��¥ ���

        // ù ��° TimeFlow�� ���۵��� �ʵ��� ����
        StartCoroutine(TimeFlow());
    }

    private void Update()
    {

        if (Instance != null && timeText != null && dayText != null)
        {
            timeText.text = GetFormattedTime();
            dayText.text = GetFormattedDay();  // ����, ����, ��¥ ���
        }
    }

    private IEnumerator TimeFlow()
    {
        Debug.Log("TimeFlow �ڷ�ƾ ���۵�");
        while (true)
        {
            yield return new WaitForSeconds(timeTick);  // timeTick�� ���� �ð��� ����ϴ� ���� �ð� �帧 �ӵ�
            Debug.Log("10�� �����");  // �帧 �׽�Ʈ�� �α�
            AdvanceTime(10);  // 10�о� ����

            if (season != null && !season.overrideSeason) // ���� �����̸� �ڵ� �������� ����
            {
                season.UpdateSeason();
            }
        }
    }

    private void AdvanceTime(int minutes)
    {
        currentMinute += minutes;

        if (currentMinute >= 60)
        {
            currentMinute -= 60;
            currentHour++;

            Debug.Log($"[�ð� ���] {CurrentWeekday} {currentHour}�� {currentMinute:D2}��");

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                season.SetCurrentDay(currentDay);

                Weather weather = FindObjectOfType<Weather>();
                if (weather != null)
                {
                    var todayWeather = weather.GetWeather(currentDay);
                    weather.ApplyWeather(todayWeather);
                }
            }

            if (currentHour >= 0 && currentHour < 6 && !isSleeping && !isFainted)
            {
                Faint();
            }
        }

        CheckSleep();
    }

    private void CheckSleep()
    {
        if (currentHour == 24 || (currentHour == 0 && currentMinute == 0))
        {
            if (!isSleeping)
            {
                Sleep();
            }
        }
    }

    private void Sleep()
    {
        isSleeping = true;
    }

    private void Faint()
    {
        isFainted = true;
    }

    // AM/PM �������� ����� �ð� ����
    public string GetFormattedTime()
    {
        int hour12 = currentHour % 12;
        if (hour12 == 0) hour12 = 12;

        string ampm = currentHour < 12 ? "AM" : "PM";

        return $"{hour12:D2}:{currentMinute:D2} {ampm}";
    }

    // Day �ؽ�Ʈ ���� (���� | ���� ��¥ ����)
    public string GetFormattedDay()
    {
        return $"{CurrentSeason} | {CurrentWeekday} {currentDay % 28 + 1}";  // currentDay + 1�� ���
    }

    private void OnValidate()
    {
        currentHour = Mathf.Clamp(currentHour, 0, 23);
        currentMinute = Mathf.Clamp(currentMinute, 0, 59);
        currentDay = Mathf.Max(0, currentDay);

        if (season != null)
        {
            season.SetCurrentDay(currentDay);

            if (overrideSeasonInInspector)
            {
                season.SetCurrentSeason(manualSeason);
            }
        }
    }
}