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

    private WeatherType lastAppliedWeather = WeatherType.Sunny;

    private Dictionary<int, WeatherType> dailyWeather = new Dictionary<int, WeatherType>();
    private Season season;

    public int currentDay = 0;

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
        Debug.Log($"오늘의 날씨: {todayWeather}");
        ApplyWeather(todayWeather);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (currentWeather != lastAppliedWeather)
        {
            ApplyWeather(currentWeather);
            lastAppliedWeather = currentWeather;
        }
#endif
    }

    public WeatherType GetWeather(int day)
    {
        if (dailyWeather.TryGetValue(day, out WeatherType weather))
        {
            return weather;
        }
        return currentWeather;
    }

    public void ApplyWeather(WeatherType weatherType)
    {
        Debug.Log($"[날씨 적용] {weatherType}");

        // 파티클 효과 반영
        EnvironmentEffect envEffect = FindObjectOfType<EnvironmentEffect>();
        if (envEffect != null)
        {
            envEffect.ApplyEffect(weatherType);
        }
        Debug.Log($"적용된 날씨: {weatherType}");

        // EnvironmentEffect 호출
        EnvironmentEffect effect = FindObjectOfType<EnvironmentEffect>();
        if (effect != null)
        {
            effect.ApplyEffect(weatherType);
        }

    }

    public void RandomSeason(Season.SeasonType seasonType)
    {
        int startDay = (int)seasonType * 28;
        dailyWeather = new Dictionary<int, WeatherType>();

        List<int> availableDays = new List<int>();
        for (int i = 0; i < 28; i++)
            availableDays.Add(startDay + i);

        if (seasonType == Season.SeasonType.Spring)
        {
            AddRandomWeather(ref availableDays, 7, WeatherType.FlowerRain);
            AddRandomWeather(ref availableDays, 5, WeatherType.Rain);
        }
        else if (seasonType == Season.SeasonType.Summer)
        {
            AddRandomWeather(ref availableDays, 8, WeatherType.Rain);
        }
        else if (seasonType == Season.SeasonType.Fall)
        {
            AddRandomWeather(ref availableDays, 5, WeatherType.Rain);
        }
        else if (seasonType == Season.SeasonType.Winter)
        {
            AddRandomWeather(ref availableDays, 7, WeatherType.Snow);
        }

        foreach (int day in availableDays)
            dailyWeather[day] = WeatherType.Sunny;
    }
    public WeatherType CurrentWeather => GetWeather(currentDay);
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
}