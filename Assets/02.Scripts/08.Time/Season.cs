using UnityEngine;

public class Season : MonoBehaviour
{
    public enum SeasonType
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    private int currentDay = 0; // TimeManager와 연동할 예정
    private readonly string[] seasonNames = { "봄", "여름", "가을", "겨울" };

    public SeasonType CurrentSeason
    {
        get
        {
            int seasonLength = 28;
            int seasonIndex = (currentDay / seasonLength) % 4;
            return (SeasonType)seasonIndex;
        }
    }

    public string CurrentSeasonName => seasonNames[(int)CurrentSeason];

    public void SetCurrentDay(int day)
    {
        currentDay = day;
    }

    public void UpdateSeason()
    {
        // 계절 업데이트를 호출할 수 있음. (필요시 추가 로직 삽입)
    }
}