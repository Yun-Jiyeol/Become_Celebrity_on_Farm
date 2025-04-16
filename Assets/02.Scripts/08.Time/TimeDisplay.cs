using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Update()
    {
        if (TimeManager.Instance == null) return;

        var tm = TimeManager.Instance;
        string weekday = tm.CurrentWeekday;
        string hour = tm.currentHour.ToString("D2");
        string minute = tm.currentMinute.ToString("D2");

        timeText.text = $"[{weekday}] {hour}:{minute}";
    }
}
