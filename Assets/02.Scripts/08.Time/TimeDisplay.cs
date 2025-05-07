using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TMP_Text timeText; // ClockGold/TimeTxt
    public TMP_Text dayText;  // ClockGold/DayTxt

    void Awake()
    {
        // 텍스트 오브젝트 자동 연결
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

        timeText.text = $"{hour}:{minute} {ampm}";

        // 날짜: 계절 | 요일 일자 형식
        string season = tm.CurrentSeason;
        string weekday = tm.CurrentWeekday;
        string day = (tm.currentDay + 1).ToString(); // 1일부터 시작하는 날짜 표현

        dayText.text = $"{season} | {weekday} {day}";
    }
}