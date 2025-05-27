using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("시간 설정")]
    public float timePerMinute = 10f;
    private float timer;

    [Header("총 누적 시간 (분)")]
    public int totalMinutes;

    [Header("현재 시간")]
    public int currentMinute;
    public int currentHour;
    public int currentDay;
    public int currentMonth; // 0~3 : 봄~겨울
    public int currentYear = 1;
    public int totalDaysPassed; // 전체 날짜 카운트 (일일 퀘스트용)

    [Header("수면 여부")]
    public bool isSleeping = false;

    [Header("계절 시스템")]
    public Season season;

    public event Action OnTimeChanged;
    public event Action OnDayChanged;
    public event Action OnMonthChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        season = FindObjectOfType<Season>();

        // 시간 초기화
        currentHour = 6;
        currentMinute = 0;
        currentDay = 0;
        currentMonth = 0;
        currentYear = 1;
        totalMinutes = currentHour * 60;

        // 계절 초기화
        if (season != null)
        {
            season.SetCurrentMonth(currentMonth); // 월 기준으로 계절 설정
        }

        // 날씨에도 날짜 초기값 전달
        if (Weather.Instance != null)
        {
            Weather.Instance.SetDay(currentDay);
        }

        OnDayChanged += GameManager.Instance.OneDayAfter;
        OnMonthChanged += GameManager.Instance.OneSeasonAfter;
    }

    private void Update()
    {
        if (isSleeping) return;

        timer += Time.deltaTime;
        if (timer >= timePerMinute)
        {
            Debug.Log($"[TimeManager] AdvanceTime 호출됨. 이전 totalMinutes: {totalMinutes}");
            AdvanceTime(10);
            timer = 0f;
        }
    }

    public void AdvanceTime(int minutes)
    {
        Debug.Log($"[TimeManager] 시간 증가 요청: {minutes}분, 이전 totalMinutes: {totalMinutes}");
        totalMinutes += minutes;
        currentMinute += minutes;

        if (currentMinute >= 60)
        {
            int extraHours = currentMinute / 60;
            currentMinute %= 60;
            currentHour += extraHours;

            if (currentHour >= 24)
            {
                int extraDays = currentHour / 24;
                currentHour %= 24;
                for (int i = 0; i < extraDays; i++)
                {
                    AdvanceDay();
                }

                // 매일 아침 6시로 초기화
                currentHour = 6;
                currentMinute = 0;

                Debug.Log($"하루 지남 - currentDay: {currentDay}, currentHour 초기화: {currentHour}");
            }
        }

        OnTimeChanged?.Invoke();
    }

    public void AdvanceDay()
    {
        // 날짜가 바뀔 때
        currentDay++;
        totalDaysPassed++; // 퀘스트용
        Debug.Log($"TimeManager - 날짜 변경: {currentDay}");

        // 퀘스트 초기화
        PlannerQuestManager.Instance?.MarkQuestAcceptedToday();

        // Weather에 날짜 알려주기
        if (Weather.Instance != null)
        {
            Weather.Instance.SetDay(currentDay);
        }
        else
        {
            Debug.LogWarning("TimeManager - Weather 인스턴스를 찾을 수 없습니다!");
        }
        Debug.Log($"AdvanceDay 호출 - currentDay 증가 후: {currentDay}");

        OnDayChanged?.Invoke();

        if (Weather.Instance != null)
        {
            Weather.Instance.SetDay(currentDay);
        }

        if (currentDay >= 7)
        {
            currentDay = 0;
            currentMonth++;
            Debug.Log($"한 달 지남 - currentMonth: {currentMonth}");

            if (currentMonth >= 4)
            {
                currentMonth = 0;
                currentYear++;
            }

            if (season != null)
            {
                season.SetCurrentMonth(currentMonth);
            }

            OnMonthChanged?.Invoke();
        }
    }

    private readonly string[] weekdays = { "월", "화", "수", "목", "금", "토", "일" };
    public string CurrentWeekday => weekdays[currentDay % 7];
    public string CurrentSeason => season != null ? season.CurrentSeasonName : "봄";
}