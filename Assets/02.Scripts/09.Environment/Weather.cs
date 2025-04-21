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

    // ���� ������ �ν����Ϳ��� Ȯ���� �� �ֵ��� ����
    [SerializeField]
    private WeatherType currentWeather = WeatherType.Sunny;  // �⺻ ������ Sunny�� ����

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
        // �ν����Ϳ��� �������� ������ ������ �ٷ� �����ϵ��� ����
        WeatherType todayWeather = GetWeather(currentDay);
        Debug.Log($"������ ����: {todayWeather}");

        // ������ ������ �ٷ� ����
        ApplyWeather(todayWeather);
    }

    public WeatherType GetWeather(int day)
    {
        // �ش� ��¥�� ������ ���� ��ȯ
        if (dailyWeather.TryGetValue(day, out WeatherType weather))
        {
            return weather;
        }
        return currentWeather;  // �⺻ ���� (�������� ������ �� ���)
    }

    public void ApplyWeather(WeatherType weatherType)
    {
        // ������ �����ϴ� ����
        Debug.Log($"����� ����: {weatherType}");
        // ���⿡ ������ �´� ��ƼŬ ȿ�� ���� �ڵ� �߰�
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

        // �������� Sunny ó��
        foreach (int day in availableDays)
            dailyWeather[day] = WeatherType.Sunny;
    }

    private void AddRandomWeather(ref List<int> pool, int count, WeatherType type)
    {
        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count); //��¥ �� �ϳ� ���� ��
            int selectedDay = pool[index];//�ش� ��¥ ����
            dailyWeather[selectedDay] = type;//�ش� ��¥�� ���� ����
            pool.RemoveAt(index);//�ߺ� ����
        }
    }
}