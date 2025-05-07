using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("�ð� ����")]
    public float timePerMinute = 1f;
    private float timer;

    [Header("���� �ð� ����")]
    public int currentMinute;
    public int currentHour;
    public int currentDay;
    public int currentMonth; // 0~3 : ��~�ܿ�
    public int currentYear = 1;

    [Header("����")]
    public bool isSleeping = false;

    [Header("����� ���� �ý���")]
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

    private void Update()
    {
        if (isSleeping) return;

        timer += Time.deltaTime;
        if (timer >= timePerMinute)
        {
            AdvanceTime(10); // 10�ʿ� 10�� �帧
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
            if (currentHour >= 24)
            {
                currentHour = 6;
                AdvanceDay();
            }
        }

        OnTimeChanged?.Invoke();
    }

    public void AdvanceDay()
    {
        currentDay++;

        if (season != null)
            season.SetCurrentDay(currentDay);

        OnDayChanged?.Invoke();

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

    private readonly string[] weekdays = { "��", "ȭ", "��", "��", "��", "��", "��" };
    public string CurrentWeekday => weekdays[currentDay % 7];
    public string CurrentSeason => season != null ? season.CurrentSeasonName : "��";
}