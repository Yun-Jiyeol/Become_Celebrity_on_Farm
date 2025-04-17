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

    // 눈 스프라이트 관련 변수 추가
    public GameObject snowSpritePrefab;
    public int snowSpriteCount = 5;

    // 오늘 날짜 (예시로 0일차로 가정, 나중에 TimeManager와 연동 가능)
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
            CreateSnowSprites(); // 눈 내리는 효과 실행
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

    //눈 스프라이트 계단식 출력 함수
    private void CreateSnowSprites()
    {
        if (snowSpritePrefab == null)
        {
            Debug.LogWarning("Snow Sprite Prefab이 연결되지 않았습니다!");
            return;
        }

        for (int i = 0; i < snowSpriteCount; i++)
        {
            Vector3 pos = new Vector3(i * 1.0f, -i * 0.5f, 0); // 계단식 위치
            Instantiate(snowSpritePrefab, pos, Quaternion.identity, transform);
        }
    }
}