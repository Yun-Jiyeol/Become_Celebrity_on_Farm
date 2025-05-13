using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public static Weather Instance { get; private set; }

    public enum WeatherType
    {
        Sunny,
        Rain,
        Snow,
        FlowerRain
    }

    [SerializeField]
    private WeatherType currentWeather = WeatherType.Sunny;
    public WeatherType CurrentWeather => currentWeather;

    private Dictionary<int, WeatherType> dailyWeather = new Dictionary<int, WeatherType>();
    private Season season;

    public int currentDay = 0;

    private EnvironmentEffect envEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        envEffect = GetComponent<EnvironmentEffect>();
        season = FindObjectOfType<Season>();
        if (season != null)
        {
            RandomSeason(season.CurrentSeason);
            season.OnSeasonChanged += RandomSeason;
        }
    }

    private void Start()
    {
        WeatherType todayWeather = GetWeather(currentDay);
        ApplyWeather(todayWeather);
    }

    public WeatherType GetWeather(int day)
    {
        if (dailyWeather.TryGetValue(day, out WeatherType weather))
        {
            return weather;
        }
        return WeatherType.Sunny;
    }

    public void ApplyWeather(WeatherType weatherType)
    {
        currentWeather = weatherType;
        envEffect.ApplyEffect(weatherType, true); // 기본은 실외
    }

    public void ApplyWeather(WeatherType weatherType, bool isOutside)
    {
        currentWeather = weatherType;
        envEffect.ApplyEffect(weatherType, isOutside);
    }

    public void SetDay(int day)
    {
        currentDay = day;
        WeatherType newWeather = GetWeather(currentDay);
        ApplyWeather(newWeather);
    }

    public void RandomSeason(Season.SeasonType seasonType)
    {
        dailyWeather = new Dictionary<int, WeatherType>();

        List<int> availableDays = new List<int>();
        for (int i = 0; i < 7; i++) // 항상 0~6 범위만 사용
            availableDays.Add(i);

        // 계절별 특수 날씨 설정
        if (seasonType == Season.SeasonType.Spring)
        {
            AddRandomWeather(ref availableDays, 2, WeatherType.FlowerRain);
            AddRandomWeather(ref availableDays, 2, WeatherType.Rain);
        }
        else if (seasonType == Season.SeasonType.Summer)
        {
            AddRandomWeather(ref availableDays, 2, WeatherType.Rain);
        }
        else if (seasonType == Season.SeasonType.Fall)
        {
            AddRandomWeather(ref availableDays, 2, WeatherType.Rain);
        }
        else if (seasonType == Season.SeasonType.Winter)
        {
            AddRandomWeather(ref availableDays, 3, WeatherType.Snow);
        }

        // 남은 날은 Sunny로 채움
        foreach (int day in availableDays)
        {
            dailyWeather[day] = WeatherType.Sunny;
        }
    }

    private void AddRandomWeather(ref List<int> pool, int count, WeatherType type)
    {
        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            int selectedDay = pool[index];
            dailyWeather[selectedDay] = type;
            pool.RemoveAt(index);
        }
    }

    public void HideWeatherEffect()
    {
        envEffect.ApplyEffect(currentWeather, false); // false → 실외 아님 (비활성화)
    }
}