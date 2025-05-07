using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TMP_Text timeText; // ClockGold/TimeTxt
    public TMP_Text dayText;  // ClockGold/DayTxt

    void Awake()
    {
        // �ؽ�Ʈ ������Ʈ �ڵ� ����
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

        // AM/PM �������� �ð� ǥ��
        string hour = tm.currentHour % 12 == 0 ? "12" : (tm.currentHour % 12).ToString("D2");
        string minute = tm.currentMinute.ToString("D2");
        string ampm = tm.currentHour < 12 ? "AM" : "PM";

        timeText.text = $"{hour}:{minute} {ampm}";

        // ��¥: ���� | ���� ���� ����
        string season = tm.CurrentSeason;
        string weekday = tm.CurrentWeekday;
        string day = (tm.currentDay + 1).ToString(); // 1�Ϻ��� �����ϴ� ��¥ ǥ��

        dayText.text = $"{season} | {weekday} {day}";
    }
}