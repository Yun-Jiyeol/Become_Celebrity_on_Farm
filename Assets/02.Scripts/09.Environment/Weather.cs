using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public enum WeatherType
    {
        Sunny,
        Rain,
        Snow
    }

    private Dictionary<int, WeatherType> dailyWeather = new Dictionary<int, WeatherType>();
    private Season season;

    // �� ��������Ʈ ���� ���� �߰�
    public GameObject snowSpritePrefab;
    public int snowSpriteCount = 5;

    // ���� ��¥ (���÷� 0������ ����, ���߿� TimeManager�� ���� ����)
    public int currentDay = 0;

    private void Awake()
    {
        season = FindObjectOfType<Season>();

        if (season != null)
        {
            GenerateWeatherForSeason(season.CurrentSeason);
            season.OnSeasonChanged += GenerateWeatherForSeason;
        }
    }

    private void Start()
    {
        WeatherType todayWeather = GetWeather(currentDay);

        if (todayWeather == WeatherType.Snow)
        {
            CreateSnowSprites(); // �� ������ ȿ�� ����
        }
    }

    public WeatherType GetWeather(int day)
    {
        if (dailyWeather.TryGetValue(day, out WeatherType weather))
        {
            return weather;
        }

        return WeatherType.Sunny;
    }

    public void GenerateWeatherForSeason(Season.SeasonType seasonType)
    {
        int startDay = GetSeasonStartDay(seasonType);
        dailyWeather = new Dictionary<int, WeatherType>(dailyWeather);

        switch (seasonType)
        {
            case Season.SeasonType.Spring:
                SetRandomWeather(startDay, 28, 5, WeatherType.Rain);
                break;
            case Season.SeasonType.Summer:
                SetRandomWeather(startDay, 28, 8, WeatherType.Rain);
                break;
            case Season.SeasonType.Fall:
                SetRandomWeather(startDay, 28, 5, WeatherType.Rain);
                break;
            case Season.SeasonType.Winter:
                SetRandomWeather(startDay, 28, 7, WeatherType.Snow);
                break;
        }
    }

    private void SetRandomWeather(int startDay, int range, int count, WeatherType type)
    {
        List<int> randomDays = GetRandomDays(startDay, range, count);

        foreach (int day in randomDays)
        {
            dailyWeather[day] = type;
        }
    }

    private int GetSeasonStartDay(Season.SeasonType seasonType)
    {
        return (int)seasonType * 28;
    }

    private List<int> GetRandomDays(int startDay, int range, int count)
    {
        List<int> allDays = new List<int>();
        for (int i = 0; i < range; i++)
        {
            allDays.Add(startDay + i);
        }

        List<int> result = new List<int>();
        for (int i = 0; i < count && allDays.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, allDays.Count);
            result.Add(allDays[randomIndex]);
            allDays.RemoveAt(randomIndex);
        }

        return result;
    }

    //�� ��������Ʈ ��ܽ� ��� �Լ�
    private void CreateSnowSprites()
    {
        if (snowSpritePrefab == null)
        {
            Debug.LogWarning("Snow Sprite Prefab�� ������� �ʾҽ��ϴ�!");
            return;
        }

        for (int i = 0; i < snowSpriteCount; i++)
        {
            Vector3 pos = new Vector3(i * 1.0f, -i * 0.5f, 0); // ��ܽ� ��ġ
            Instantiate(snowSpritePrefab, pos, Quaternion.identity, transform);
        }
    }
}