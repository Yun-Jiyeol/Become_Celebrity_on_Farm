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
            }
        }

        OnTimeChanged?.Invoke();
        QuestManager.Instance?.OnAdvanceDay();

    }

    public void AdvanceDay()
    {
        currentDay++;

        OnDayChanged?.Invoke();

        // 날씨에 날짜 반영
        if (Weather.Instance != null)
        {
            Weather.Instance.SetDay(currentDay);
        }

        // 7일 지나면 한 달
        if (currentDay >= 7)
        {
            currentDay = 0;
            currentMonth++;

            // 월이 4 넘어가면 다음 해
            if (currentMonth >= 4)
            {
                currentMonth = 0;
                currentYear++;
            }

            // 계절 갱신 (월 기준)
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


//using UnityEngine;
//using System;

//public class TimeManager : MonoBehaviour
//{
//    public static TimeManager Instance;

//    [Header("timePerMinute")]
//    public float timePerMinute = 10f;
//    private float timer;

//    [Header("time")]
//    public int currentMinute;
//    public int currentHour;
//    public int currentDay;
//    public int currentMonth; // 0~3 : 봄~겨울
//    public int currentYear = 1;

//    [Header("sleep")]
//    public bool isSleeping = false;

//    [Header("season")]
//    public Season season;

//    public event Action OnTimeChanged;
//    public event Action OnDayChanged;
//    public event Action OnMonthChanged;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        currentHour = 6;
//        currentMinute = 0;

//        if (season != null)
//        {
//            season.SetCurrentDay(currentDay);
//            currentMinute = 0;
//            currentHour = 6;
//            currentDay = 0;
//            currentMonth = 0;
//            currentYear = 1;

//            UpdateSeason();
//        }
//    }

//    private void Update()
//    {
//        if (isSleeping) return;

//        timer += Time.deltaTime;
//        if (timer >= timePerMinute)
//        {
//            AdvanceTime(10);
//            timer = 0f;
//        }
//    }

//    public void AdvanceTime(int minutes)
//    {
//        currentMinute += minutes;

//        if (currentMinute >= 60)
//        {
//            currentMinute = 0;
//            currentHour++;
//            //if (currentHour >= 24)
//            //{
//            //    currentHour = 6;
//            //    AdvanceDay();
//            //}
//        }

//        OnTimeChanged?.Invoke();
//    }

//    public void AdvanceDay()
//    {
//        currentDay++;

//        if (season != null)
//            season.SetCurrentDay(currentDay);

//        OnDayChanged?.Invoke();

//        QuestManager.Instance?.OnAdvanceDay();

//        if (currentDay >= 28)
//        {
//            currentDay = 0;
//            currentMonth++;
//            if (currentMonth >= 4)
//            {
//                currentMonth = 0;
//                currentYear++;
//            }

//            OnMonthChanged?.Invoke();
//        }
//    }

//    public void UpdateSeason()
//    {
//        if (season != null)
//            season.SetCurrentDay(currentDay);
//    }

//    private readonly string[] weekdays = { "월", "화", "수", "목", "금", "토", "일" };
//    public string CurrentWeekday => weekdays[currentDay % 7];
//    public string CurrentSeason => season != null ? season.CurrentSeasonName : "봄";
//}