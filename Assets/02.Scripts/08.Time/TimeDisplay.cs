using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TMP_Text timeText;  // TimeTxt 텍스트
    public TMP_Text dayText;   // DayTxt 텍스트

    void Awake()
    {
        // TimeTxt와 DayTxt 연결
        if (timeText == null)
        {
            var timeTransform = transform.root.Find("Canvases/InGameCanvas/PlayerInGameUI/ClockGold/TimeTxt");
            if (timeTransform != null)
                timeText = timeTransform.GetComponent<TMP_Text>();
        }

        if (dayText == null)
        {
            var dayTransform = transform.root.Find("Canvases/InGameCanvas/PlayerInGameUI/ClockGold/DayTxt");
            if (dayTransform != null)
                dayText = dayTransform.GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        if (TimeManager.Instance == null) return;

        var tm = TimeManager.Instance;

        // AM/PM 형식으로 시간 표시
        string hour = tm.currentHour % 12 == 0 ? "12" : (tm.currentHour % 12).ToString("D2");
        string minute = tm.currentMinute.ToString("D2");
        string ampm = tm.currentHour < 12 ? "AM" : "PM";

        timeText.text = $"{hour}:{minute} {ampm}";  // TimeTxt

        // Day 텍스트 포맷 (계절 | 요일 날짜 형식)
        string season = tm.CurrentSeason;
        string weekday = tm.CurrentWeekday;
        string day = (tm.currentDay + 1).ToString();  // 날짜는 1일부터 시작

        dayText.text = $"{season} | {weekday} {day}";  // DayTxt
    }
}