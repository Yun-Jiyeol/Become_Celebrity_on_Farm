using TMPro;
using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public TMP_Text timeText;   // TimeTxt 텍스트
    public TMP_Text dayText;    // DayTxt 텍스트

    [Header("현재 시간 수동 설정 (테스트용)")]
    [Range(0, 23)] public int currentHour = 6;
    [Range(0, 59)] public int currentMinute = 0;
    [Range(0, 365)] public int currentDay = 0; // 0 = 월요일

    private readonly string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    private readonly string[] seasons = { "봄", "여름", "가을", "겨울" };

    public string CurrentWeekday => weekdays[currentDay % 7];
    public string CurrentSeason => seasons[(currentDay / 28) % 4];  // 계절 계산

    private bool isSleeping = false;
    private bool isFainted = false;

    [Header("Time Settings")]
    [Tooltip("현실 몇 초마다 게임 시간 10분이 흐를지 설정")]
    public float realSecondsPerGameTenMinutes = 10f;

    private float timeTick => realSecondsPerGameTenMinutes;

    private Season season;

    [Header("계절 수동 설정 (TimeManager → Season)")]
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

        // Inspector에서 연결되지 않았을 경우 자동으로 찾기
        if (timeText == null)
        {
            var timeTransform = transform.root.Find("Canvases/InGameCanvas/PlayerInGameUI/ClockGold/TimeTxt");
            if (timeTransform != null)
                timeText = timeTransform.GetComponent<TMP_Text>();
            else
                Debug.LogWarning("TimeTxt를 찾을 수 없습니다. Inspector에 직접 연결해 주세요.");
        }

        if (dayText == null)
        {
            var dayTransform = transform.root.Find("Canvases/InGameCanvas/PlayerInGameUI/ClockGold/DayTxt");
            if (dayTransform != null)
                dayText = dayTransform.GetComponent<TMP_Text>();
            else
                Debug.LogWarning("DayTxt를 찾을 수 없습니다. Inspector에 직접 연결해 주세요.");
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

        // 시작 시 첫 번째 타임플로우 시작 전에 초기 시간을 표시하도록 처리
        timeText.text = GetFormattedTime();
        dayText.text = GetFormattedDay();  // 계절, 요일, 날짜 출력

        // 첫 번째 TimeFlow가 시작되지 않도록 수정
        StartCoroutine(TimeFlow());
    }

    private void Update()
    {

        if (Instance != null && timeText != null && dayText != null)
        {
            timeText.text = GetFormattedTime();
            dayText.text = GetFormattedDay();  // 계절, 요일, 날짜 출력
        }
    }

    private IEnumerator TimeFlow()
    {
        Debug.Log("TimeFlow 코루틴 시작됨");
        while (true)
        {
            yield return new WaitForSeconds(timeTick);  // timeTick은 현실 시간에 비례하는 게임 시간 흐름 속도
            Debug.Log("10분 경과됨");  // 흐름 테스트용 로그
            AdvanceTime(10);  // 10분씩 증가

            if (season != null && !season.overrideSeason) // 수동 계절이면 자동 갱신하지 않음
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

    // Day 텍스트 포맷 (계절 | 요일 날짜 형식)
    public string GetFormattedDay()
    {
        return $"{CurrentSeason} | {CurrentWeekday} {currentDay % 28 + 1}";  // currentDay + 1로 출력
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