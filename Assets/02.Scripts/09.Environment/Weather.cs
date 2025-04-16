using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public enum WeatherType
    {
        Sunny, // 맑음
        Rain,  // 비
        Snow   // 눈
    }

    // 하루별 날씨 저장 딕셔너리 (key: 날짜, value: 날씨 타입)
    private Dictionary<int, WeatherType> dailyWeather = new Dictionary<int, WeatherType>();

    private Season season;

    private void Awake()
    {
        // Season 스크립트 찾기
        season = FindObjectOfType<Season>();

        if (season != null)
        {
            // 처음 시작 시 현재 계절 기준으로 날씨 생성
            GenerateWeatherForSeason(season.CurrentSeason);

            // 계절이 바뀌면 자동으로 날씨 재생성
            season.OnSeasonChanged += GenerateWeatherForSeason;
        }
    }

    // 해당 날짜의 날씨를 가져오는 함수
    public WeatherType GetWeather(int day)
    {
        if (dailyWeather.TryGetValue(day, out WeatherType weather))
        {
            return weather;
        }

        return WeatherType.Sunny; // 기본값은 맑음
    }

    // 주어진 계절에 맞춰 날씨 생성
    public void GenerateWeatherForSeason(Season.SeasonType seasonType)
    {
        int startDay = GetSeasonStartDay(seasonType); // 해당 계절의 시작일

        // 기존 날씨 데이터를 유지하면서 새로운 날씨 추가
        dailyWeather = new Dictionary<int, WeatherType>(dailyWeather);

        switch (seasonType)
        {
            case Season.SeasonType.Spring:
                SetRandomWeather(startDay, 28, 5, WeatherType.Rain); // 봄: 28일 중 5일 비
                break;
            case Season.SeasonType.Summer:
                SetRandomWeather(startDay, 28, 8, WeatherType.Rain); // 여름: 8일 비
                break;
            case Season.SeasonType.Fall:
                SetRandomWeather(startDay, 28, 5, WeatherType.Rain); // 가을: 5일 비
                break;
            case Season.SeasonType.Winter:
                SetRandomWeather(startDay, 28, 7, WeatherType.Snow); // 겨울: 7일 눈
                break;
        }
    }

    // 특정 범위 내 랜덤 날짜에 지정된 날씨를 설정
    private void SetRandomWeather(int startDay, int range, int count, WeatherType type)
    {
        List<int> randomDays = GetRandomDays(startDay, range, count); // 랜덤한 날짜 뽑기

        foreach (int day in randomDays)
        {
            dailyWeather[day] = type; // 해당 날짜에 날씨 지정
        }
    }

    // 계절 시작 날짜 계산 (봄: 0, 여름: 28, 가을: 56, 겨울: 84)
    private int GetSeasonStartDay(Season.SeasonType seasonType)
    {
        return (int)seasonType * 28;
    }

    // 주어진 범위 내에서 중복 없는 랜덤 날짜 뽑기
    private List<int> GetRandomDays(int startDay, int range, int count)
    {
        List<int> allDays = new List<int>();
        for (int i = 0; i < range; i++)
        {
            allDays.Add(startDay + i); // 예: 28일 간의 날짜 리스트
        }

        List<int> result = new List<int>();
        for (int i = 0; i < count && allDays.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, allDays.Count); // 랜덤하게 인덱스 선택
            result.Add(allDays[randomIndex]); // 해당 날짜 추가
            allDays.RemoveAt(randomIndex);    // 중복 방지를 위해 제거
        }

        return result;
    }
}