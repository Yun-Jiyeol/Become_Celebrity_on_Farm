using TMPro;
using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public TMP_Text timeText;

    [Header("현재 시간 수동 설정 (테스트용)")]
    [Range(0, 23)] public int currentHour = 6;
    [Range(0, 59)] public int currentMinute = 0;
    [Range(0, 365)] public int currentDay = 0; // 0 = 월요일

    private readonly string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    public string CurrentWeekday => weekdays[currentDay % 7];

    private bool isSleeping = false;
    private bool isFainted = false;

    [Header("Time Settings")]
    [Tooltip("현실 몇 초마다 게임 시간 10분이 흐를지 설정")]
    public float realSecondsPerGameTenMinutes = 10f;

    private float timeTick => realSecondsPerGameTenMinutes;

    private Season season;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        season = FindObjectOfType<Season>();
    }

    private void Start()
    {
        if (season != null)
        {
            season.SetCurrentDay(currentDay);
        }
        StartCoroutine(TimeFlow());
    }

    private void Update()
    {
        if (Instance != null && timeText != null)
        {
            timeText.text = GetFormattedTime();
        }
    }

    private IEnumerator TimeFlow()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeTick);
            AdvanceTime(10);

            if (season != null)
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

            Debug.Log($"[시간 경과] {CurrentWeekday} {currentHour}시 {currentMinute:D2}분");

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

    // AM/PM 형식으로 변경된 시간 포맷
    public string GetFormattedTime()
    {
        int hour12 = currentHour % 12;
        if (hour12 == 0) hour12 = 12;

        string ampm = currentHour < 12 ? "AM" : "PM";

        return $"{hour12:D2}:{currentMinute:D2} {ampm}";
    }

    private void OnValidate()
    {
        currentHour = Mathf.Clamp(currentHour, 0, 23);
        currentMinute = Mathf.Clamp(currentMinute, 0, 59);
        currentDay = Mathf.Max(0, currentDay);

        if (season != null)
        {
            season.SetCurrentDay(currentDay);
        }
    }
}