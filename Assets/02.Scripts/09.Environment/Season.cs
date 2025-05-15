using UnityEngine;
using System;

public class Season : MonoBehaviour
{
    public enum SeasonType
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    [Header("수동 계절 설정 (테스트용)")]
    public bool overrideSeason = false;
    public SeasonType manualSeason;

    private int currentMonth = 0; // TimeManager와 연동
    private readonly string[] seasonNames = { "봄", "여름", "가을", "겨울" };

    private SeasonType lastSeason = SeasonType.Spring;

    public event Action<SeasonType> OnSeasonChanged; // 계절 변경 이벤트

    public SeasonType CurrentSeason
    {
        get
        {
            if (overrideSeason)
            {
                return manualSeason;
            }

            // 계절 계산은 currentMonth 기준
            int seasonIndex = currentMonth % 4;
            return (SeasonType)seasonIndex;
        }
    }

    public string CurrentSeasonName => seasonNames[(int)CurrentSeason];

    // TimeManager에서 월(month)을 업데이트할 때 호출
    public void SetCurrentMonth(int month)
    {
        currentMonth = month;
        UpdateSeason();
    }

    // 테스트나 디버깅용 강제 설정
    public void SetCurrentSeason(SeasonType season)
    {
        overrideSeason = true;
        manualSeason = season;
        UpdateSeason();
    }

    // 계절이 바뀌면 이벤트 발생
    public void UpdateSeason()
    {
        SeasonType current = CurrentSeason;
        if (current != lastSeason)
        {
            lastSeason = current;
            OnSeasonChanged?.Invoke(current);
        }
    }
}

//using UnityEngine;
//using System;

//public class Season : MonoBehaviour
//{
//    public enum SeasonType
//    {
//        Spring,
//        Summer,
//        Fall,
//        Winter
//    }

//    [Header("수동 계절 설정 (테스트용)")]
//    public bool overrideSeason = false;
//    public SeasonType manualSeason;

//    private int currentMonth = 0; // TimeManager와 연동할 예정
//    private readonly string[] seasonNames = { "봄", "여름", "가을", "겨울" };

//    private SeasonType lastSeason = SeasonType.Spring;

//    public event Action<SeasonType> OnSeasonChanged; // 계절 변경 이벤트 추가

//    public SeasonType CurrentSeason
//    {
//        get
//        {
//            if (overrideSeason)
//            {
//                return manualSeason;
//            }
//            int seasonLength = 7;
//            //int seasonLength = 28;
//            int seasonIndex = (currentDay / seasonLength) % 4;
//            return (SeasonType)seasonIndex;
//        }
//    }

//    public string CurrentSeasonName => seasonNames[(int)CurrentSeason];

//    public void SetCurrentDay(int day)
//    {
//        currentDay = day;
//        UpdateSeason(); // 날짜가 바뀌면 시즌 업데이트 호출
//    }

//    public void SetCurrentSeason(SeasonType season)
//    {
//        overrideSeason = true;
//        manualSeason = season;
//        UpdateSeason();
//    }

//    public void UpdateSeason()
//    {
//        SeasonType current = CurrentSeason;
//        if (current != lastSeason)
//        {
//            lastSeason = current;
//            OnSeasonChanged?.Invoke(current); // 계절이 바뀌었으면 이벤트 발동
//        }
//    }
//}


//using UnityEngine;
//using System;

//public class Season : MonoBehaviour
//{
//    public enum SeasonType
//    {
//        Spring,
//        Summer,
//        Fall,
//        Winter
//    }

//    [Header("수동 계절 설정 (테스트용)")]
//    public bool overrideSeason = false;
//    public SeasonType manualSeason;

//    private int currentDay = 0; // TimeManager와 연동할 예정
//    private readonly string[] seasonNames = { "봄", "여름", "가을", "겨울" };

//    private SeasonType lastSeason = SeasonType.Spring;

//    public event Action<SeasonType> OnSeasonChanged; // 계절 변경 이벤트 추가

//    public SeasonType CurrentSeason
//    {
//        get
//        {
//            if (overrideSeason)
//            {
//                return manualSeason;
//            }

//            int seasonLength = 28;
//            int seasonIndex = (currentDay / seasonLength) % 4;
//            return (SeasonType)seasonIndex;
//        }
//    }

//    public string CurrentSeasonName => seasonNames[(int)CurrentSeason];

//    public void SetCurrentDay(int day)
//    {
//        currentDay = day;
//        UpdateSeason(); // 날짜가 바뀌면 시즌 업데이트 호출
//    }

//    public void SetCurrentSeason(SeasonType season)
//    {
//        overrideSeason = true;
//        manualSeason = season;
//        UpdateSeason();
//    }

//    public void UpdateSeason()
//    {
//        SeasonType current = CurrentSeason;
//        if (current != lastSeason)
//        {
//            lastSeason = current;
//            OnSeasonChanged?.Invoke(current); // 계절이 바뀌었으면 이벤트 발동
//        }
//    }
//}
