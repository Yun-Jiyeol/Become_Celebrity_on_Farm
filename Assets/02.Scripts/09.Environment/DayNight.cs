using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] public SpriteRenderer backgroundRenderer; // 하늘 배경 SpriteRenderer
    public Color dayColor = new Color(0.8f, 0.9f, 1f);  // 밝은 하늘색
    public Color nightColor = new Color(0.1f, 0.1f, 0.2f); // 어두운 남색

    [Range(0, 24)]
    public int dayStartHour = 6;   // 아침 6시
    [Range(0, 24)]
    public int nightStartHour = 18; // 저녁 6시

    private void Update()
    {
        if (TimeManager.Instance == null || backgroundRenderer == null)
            return;

        int currentHour = TimeManager.Instance.currentHour;

        // 0~1 사이의 보간값 계산
        float t = 0f;
        if (currentHour >= dayStartHour && currentHour < nightStartHour)
        {
            // 낮 (dayStartHour ~ nightStartHour)
            t = Mathf.InverseLerp(dayStartHour, nightStartHour, currentHour);
            backgroundRenderer.color = Color.Lerp(dayColor, nightColor, t);
        }
        else
        {
            // 밤 (nightStartHour ~ 다음날 dayStartHour)
            int hour = currentHour;
            if (currentHour < dayStartHour) hour += 24; // 새벽 시간 조정

            t = Mathf.InverseLerp(nightStartHour, dayStartHour + 24, hour);
            backgroundRenderer.color = Color.Lerp(nightColor, dayColor, t);
        }
    }
}

