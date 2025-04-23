using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("���� �ð� ���� ���� (�׽�Ʈ��)")]
    [Range(0, 23)] public int currentHour = 6;
    [Range(0, 59)] public int currentMinute = 0;
    [Range(0, 365)] public int currentDay = 0; // 0 = ������

    private readonly string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    public string CurrentWeekday => weekdays[currentDay % 7];

    private bool isSleeping = false;
    private bool isFainted = false;

    [Header("Time Settings")]
    [Tooltip("���� �� �ʸ��� ���� �ð� 10���� �带�� ����")]
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

            Debug.Log($"[�ð� ���] {CurrentWeekday} {currentHour}�� {currentMinute:D2}��");

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                season.SetCurrentDay(currentDay);

                ///�Ϸ簡 ������ ���� �ڵ� ����!
                Weather weather = FindObjectOfType<Weather>();
                if (weather != null)
                {
                    var todayWeather = weather.GetWeather(currentDay);
                    weather.ApplyWeather(todayWeather); // ���� �� ��ƼŬ �ݿ�
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

    public string GetFormattedTime()
    {
        return $"{CurrentWeekday} {currentHour:D2}:{currentMinute:D2}";
    }

    // �ν����Ϳ��� �� ���� �� ȣ��� (������ ����)
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