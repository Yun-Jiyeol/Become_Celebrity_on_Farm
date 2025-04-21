using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public enum WeatherType
    {
        Sunny,
        Rain,
        Snow,
        FlowerRain
    }

    private Dictionary<int, WeatherType> dailyWeather = new Dictionary<int, WeatherType>();
    private Season season;

    public int currentDay = 0;

    private void Awake()
    {
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
    }

    public WeatherType GetWeather(int day)
    {
        if (dailyWeather.TryGetValue(day, out WeatherType weather))
        {
            return weather;
        }
        return WeatherType.Sunny;
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

        // 나머지는 Sunny 처리
        foreach (int day in availableDays)
            dailyWeather[day] = WeatherType.Sunny;
    }

    private void AddRandomWeather(ref List<int> pool, int count, WeatherType type)
    {
        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count); //날짜 중 하나 랜덤 픽
            int selectedDay = pool[index];//해당 날짜 선택
            dailyWeather[selectedDay] = type;//해당 날짜에 날씨 지정
            pool.RemoveAt(index);//중복 방지
        }
    }
}