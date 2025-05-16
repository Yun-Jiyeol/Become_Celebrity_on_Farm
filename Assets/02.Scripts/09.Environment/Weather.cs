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

    [SerializeField] private int currentDayDebug = 0;         // �ν����Ϳ�
    [SerializeField] private WeatherType currentWeatherDebug; // �ν����Ϳ�

    // ���� ��¥�� TimeManager���� SetDay ȣ�� �� �ܺο��� ������
    private int currentDay = 0;

    [SerializeField] private EnvironmentEffect envEffect; // �ν����Ϳ��� �Ҵ� �����ϵ��� ����

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

        Debug.Log("Weather Awake: �̱��� �ν��Ͻ� ���� �Ϸ�");

        season = FindObjectOfType<Season>();
        if (season != null)
        {
            Debug.Log($"Weather Awake - Found Season: {season.CurrentSeason}");
            RandomSeason(season.CurrentSeason);
            season.OnSeasonChanged += RandomSeason;
        }
        else
        {
            Debug.LogWarning("Weather Awake - Season ������Ʈ�� ã�� ���߽��ϴ�!");
        }

        // envEffect�� �ν����Ϳ� ������ �ڵ����� ã���� �õ� (ȯ��Ŵ����� �پ��ִٰ� ����)
        if (envEffect == null)
        {
            envEffect = GetComponent<EnvironmentEffect>();
            if (envEffect == null)
            {
                Debug.LogWarning("Weather: EnvironmentEffect ������Ʈ�� ã�� ���߽��ϴ�!");
            }
            else
            {
                Debug.Log("Weather: EnvironmentEffect �ڵ� �Ҵ� �Ϸ�");
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
            Debug.LogWarning("ApplyWeather: EnvironmentEffect�� �Ҵ���� �ʾҽ��ϴ�. ȿ���� ������ �� �����ϴ�.");
            return;
        }

        envEffect.ApplyEffect(weatherType, true); // �⺻�� �ǿ�
    }

    public void ApplyWeather(WeatherType weatherType, bool isOutside)
    {
        Debug.Log($"ApplyWeather called with: {weatherType}, isOutside: {isOutside}");
        currentWeather = weatherType;

        if (envEffect == null)
        {
            Debug.LogWarning("ApplyWeather: EnvironmentEffect�� �Ҵ���� �ʾҽ��ϴ�. ȿ���� ������ �� �����ϴ�.");
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

        Debug.Log($"SetDay ȣ��� - Day: {day}, ����: {newWeather}");

        ApplyWeather(newWeather);
    }

    public void RandomSeason(Season.SeasonType seasonType)
    {
        Debug.Log("RandomSeason called with: " + seasonType);
        dailyWeather = new Dictionary<int, WeatherType>();

        List<int> availableDays = new List<int>();
        for (int i = 0; i < 7; i++) // �׻� 0~6 ������ ���
            availableDays.Add(i);

        // ������ Ư�� ���� ����
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

        // ���� ���� Sunny�� ä��
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
            Debug.LogWarning("HideWeatherEffect: EnvironmentEffect�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }
        envEffect.ApplyEffect(currentWeather, false); // false �� �ǿ� �ƴ� (��Ȱ��ȭ)
    }
}

