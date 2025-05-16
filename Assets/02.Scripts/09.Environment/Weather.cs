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

    [SerializeField] private int currentDayDebug = 0;         // 인스펙터용
    [SerializeField] private WeatherType currentWeatherDebug; // 인스펙터용

    // 현재 날짜는 TimeManager에서 SetDay 호출 시 외부에서 설정됨
    private int currentDay = 0;

    [SerializeField] private EnvironmentEffect envEffect; // 인스펙터에서 할당 가능하도록 변경

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("Weather Awake: 싱글톤 인스턴스 생성 완료");

        season = FindObjectOfType<Season>();
        if (season != null)
        {
            Debug.Log($"Weather Awake - Found Season: {season.CurrentSeason}");
            RandomSeason(season.CurrentSeason);
            season.OnSeasonChanged += RandomSeason;
        }
        else
        {
            Debug.LogWarning("Weather Awake - Season 컴포넌트를 찾지 못했습니다!");
        }

        // envEffect가 인스펙터에 없으면 자동으로 찾도록 시도 (환경매니저에 붙어있다고 가정)
        if (envEffect == null)
        {
            envEffect = GetComponent<EnvironmentEffect>();
            if (envEffect == null)
            {
                Debug.LogWarning("Weather: EnvironmentEffect 컴포넌트를 찾지 못했습니다!");
            }
            else
            {
                Debug.Log("Weather: EnvironmentEffect 자동 할당 완료");
            }
        }
    }

    private void Start()
    {
        Debug.Log($"Weather Start - currentDay: {currentDay}");
        WeatherType todayWeather = GetWeather(currentDay);
        Debug.Log($"Today's weather: {todayWeather}");
        ApplyWeather(todayWeather);
    }

    public WeatherType GetWeather(int day)
    {
        if (dailyWeather == null || dailyWeather.Count == 0)
        {
            Debug.LogWarning("dailyWeather is empty, regenerating based on current season.");
            if (season != null)
            {
                RandomSeason(season.CurrentSeason);
            }
            else
            {
                Debug.LogWarning("Season is null, returning Sunny as default.");
                return WeatherType.Sunny;
            }
        }

        if (dailyWeather.TryGetValue(day, out WeatherType weather))
        {
            return weather;
        }

        Debug.LogWarning($"No weather data for day {day}, returning Sunny as default.");
        return WeatherType.Sunny;
    }

    public void ApplyWeather(WeatherType weatherType)
    {
        Debug.Log($"ApplyWeather called with: {weatherType}");
        currentWeather = weatherType;

        if (envEffect == null)
        {
            Debug.LogWarning("ApplyWeather: EnvironmentEffect가 할당되지 않았습니다. 효과를 적용할 수 없습니다.");
            return;
        }

        envEffect.ApplyEffect(weatherType, true); // 기본은 실외
    }

    public void ApplyWeather(WeatherType weatherType, bool isOutside)
    {
        Debug.Log($"ApplyWeather called with: {weatherType}, isOutside: {isOutside}");
        currentWeather = weatherType;

        if (envEffect == null)
        {
            Debug.LogWarning("ApplyWeather: EnvironmentEffect가 할당되지 않았습니다. 효과를 적용할 수 없습니다.");
            return;
        }

        envEffect.ApplyEffect(weatherType, isOutside);
    }

    public void SetDay(int day)
    {
        currentDay = day;
        currentDayDebug = day;

        WeatherType newWeather = GetWeather(currentDay);
        currentWeatherDebug = newWeather;

        Debug.Log($"SetDay 호출됨 - Day: {day}, 날씨: {newWeather}");

        ApplyWeather(newWeather);
    }

    public void RandomSeason(Season.SeasonType seasonType)
    {
        Debug.Log("RandomSeason called with: " + seasonType);
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

        Debug.Log("dailyWeather contents:");
        foreach (var kvp in dailyWeather)
        {
            Debug.Log($"Day {kvp.Key}: {kvp.Value}");
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
        Debug.Log("HideWeatherEffect called");
        if (envEffect == null)
        {
            Debug.LogWarning("HideWeatherEffect: EnvironmentEffect가 할당되지 않았습니다.");
            return;
        }
        envEffect.ApplyEffect(currentWeather, false); // false → 실외 아님 (비활성화)
    }
}

