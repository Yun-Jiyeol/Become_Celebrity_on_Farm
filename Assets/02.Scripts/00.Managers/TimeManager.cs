using UnityEngine;
using System;

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("timePerMinute")]
    public float timePerMinute = 10f;
    private float timer;

    [Header("time")]
    public int currentMinute;
    public int currentHour;
    public int currentDay;
    public int currentMonth; // 0~3 : 봄~겨울
    public int currentYear = 1;

    [Header("sleep")]
    public bool isSleeping = false;

    [Header("season")]
    public Season season;

    public event Action OnTimeChanged;
    public event Action OnDayChanged;
    public event Action OnMonthChanged;

    // 현재 계절 enum
    public Season CurrentSeasonEnum => (Season)currentMonth;

    // 1부터 시작하는 계절 내 날짜 (UI용)
    public int CurrentDayInSeason => currentDay + 1;

    // 추가: 한글 시즌 이름
    public string CurrentSeasonName
    {
        get
        {
            switch (CurrentSeasonEnum)
            {
                case Season.Spring: return "봄";
                case Season.Summer: return "여름";
                case Season.Fall: return "가을";
                case Season.Winter: return "겨울";
                default: return "봄";
            }
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentHour = 6;
        currentMinute = 0;

        if (season != null)
        {
            season.SetCurrentDay(currentDay);
            currentMinute = 0;
            currentHour = 6;
            currentDay = 0;
            currentMonth = 0;
            currentYear = 1;

            UpdateSeason();
        }
    }

    private void Update()
    {
        if (isSleeping) return;

        timer += Time.deltaTime;
        if (timer >= timePerMinute)
        {
            AdvanceTime(10);
            timer = 0f;
        }
    }

    public void AdvanceTime(int minutes)
    {
        currentMinute += minutes;

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;
            //if (currentHour >= 24)
            //{
            //    currentHour = 6;
            //    AdvanceDay();
            //}
        }

        OnTimeChanged?.Invoke();
    }

    public void AdvanceDay()
    {
        currentDay++;

        if (season != null)
            season.SetCurrentDay(currentDay);

        OnDayChanged?.Invoke();

        QuestManager.Instance?.OnAdvanceDay();

        if (currentDay >= 28)
        {
            currentDay = 0;
            currentMonth++;
            if (currentMonth >= 4)
            {
                currentMonth = 0;
                currentYear++;
            }

            OnMonthChanged?.Invoke();
        }
    }

    public void UpdateSeason()
    {
        if (season != null)
            season.SetCurrentDay(currentDay);
    }

    private readonly string[] weekdays = { "월", "화", "수", "목", "금", "토", "일" };
    public string CurrentWeekday => weekdays[currentDay % 7];
    public string CurrentSeason => season != null ? season.CurrentSeasonName : "봄";
}