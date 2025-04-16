using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public enum WeatherType
    {
        Sunny, // ����
        Rain,  // ��
        Snow   // ��
    }

    // �Ϸ纰 ���� ���� ��ųʸ� (key: ��¥, value: ���� Ÿ��)
    private Dictionary<int, WeatherType> dailyWeather = new Dictionary<int, WeatherType>();

    private Season season;

    private void Awake()
    {
        // Season ��ũ��Ʈ ã��
        season = FindObjectOfType<Season>();

        if (season != null)
        {
            // ó�� ���� �� ���� ���� �������� ���� ����
            GenerateWeatherForSeason(season.CurrentSeason);

            // ������ �ٲ�� �ڵ����� ���� �����
            season.OnSeasonChanged += GenerateWeatherForSeason;
        }
    }

    // �ش� ��¥�� ������ �������� �Լ�
    public WeatherType GetWeather(int day)
    {
        if (dailyWeather.TryGetValue(day, out WeatherType weather))
        {
            return weather;
        }

        return WeatherType.Sunny; // �⺻���� ����
    }

    // �־��� ������ ���� ���� ����
    public void GenerateWeatherForSeason(Season.SeasonType seasonType)
    {
        int startDay = GetSeasonStartDay(seasonType); // �ش� ������ ������

        // ���� ���� �����͸� �����ϸ鼭 ���ο� ���� �߰�
        dailyWeather = new Dictionary<int, WeatherType>(dailyWeather);

        switch (seasonType)
        {
            case Season.SeasonType.Spring:
                SetRandomWeather(startDay, 28, 5, WeatherType.Rain); // ��: 28�� �� 5�� ��
                break;
            case Season.SeasonType.Summer:
                SetRandomWeather(startDay, 28, 8, WeatherType.Rain); // ����: 8�� ��
                break;
            case Season.SeasonType.Fall:
                SetRandomWeather(startDay, 28, 5, WeatherType.Rain); // ����: 5�� ��
                break;
            case Season.SeasonType.Winter:
                SetRandomWeather(startDay, 28, 7, WeatherType.Snow); // �ܿ�: 7�� ��
                break;
        }
    }

    // Ư�� ���� �� ���� ��¥�� ������ ������ ����
    private void SetRandomWeather(int startDay, int range, int count, WeatherType type)
    {
        List<int> randomDays = GetRandomDays(startDay, range, count); // ������ ��¥ �̱�

        foreach (int day in randomDays)
        {
            dailyWeather[day] = type; // �ش� ��¥�� ���� ����
        }
    }

    // ���� ���� ��¥ ��� (��: 0, ����: 28, ����: 56, �ܿ�: 84)
    private int GetSeasonStartDay(Season.SeasonType seasonType)
    {
        return (int)seasonType * 28;
    }

    // �־��� ���� ������ �ߺ� ���� ���� ��¥ �̱�
    private List<int> GetRandomDays(int startDay, int range, int count)
    {
        List<int> allDays = new List<int>();
        for (int i = 0; i < range; i++)
        {
            allDays.Add(startDay + i); // ��: 28�� ���� ��¥ ����Ʈ
        }

        List<int> result = new List<int>();
        for (int i = 0; i < count && allDays.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, allDays.Count); // �����ϰ� �ε��� ����
            result.Add(allDays[randomIndex]); // �ش� ��¥ �߰�
            allDays.RemoveAt(randomIndex);    // �ߺ� ������ ���� ����
        }

        return result;
    }
}