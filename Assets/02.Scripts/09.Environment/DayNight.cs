using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour
{
    [Header("밤 효과용 UI Image")]
    public Image nightOverlay;

    [Header("어둠 색깔 조정(알파값)")]
    [Range(0f, 1f)]
    public float maxAlpha = 1f;

    [Tooltip("밤으로 바뀌는 속도 (초당 변화량)")]
    public float fadeSpeed = 10f;

    private const int wakeUpHour = 6;       // 6시부터 낮
    private const int startDarkHour = 17;   // 17시부터 어두워짐
    private const int darkestHour = 1;      // 새벽 1시 가장 어두움

    void Start()
    {
        if (nightOverlay != null)
        {
            nightOverlay.color = new Color(0f, 0f, 0f, 0f); // 검정 + 투명
        }
    }

    void Update()
    {
        if (nightOverlay == null || TimeManager.Instance == null)
            return;

        int hour = TimeManager.Instance.currentHour;
        float targetAlpha = 0f;

        if (hour >= wakeUpHour && hour < startDarkHour)
        {
            // 낮
            targetAlpha = 0f;
        }
        else if (hour >= startDarkHour && hour <= 23)
        {
            // 17시 ~ 23시: 어두워짐 시작
            float t = (hour - startDarkHour) / (float)(darkestHour + 24 - startDarkHour); // 17~1시까지 총 8시간
            targetAlpha = Mathf.Lerp(0f, maxAlpha, t);
        }
        else if (hour >= 0 && hour <= darkestHour)
        {
            // 0시 ~ 1시: 어둠 마무리 도달
            float t = (hour + 24 - startDarkHour) / (float)(darkestHour + 24 - startDarkHour);
            targetAlpha = Mathf.Lerp(0f, maxAlpha, t);
        }
        else if (hour > darkestHour && hour < wakeUpHour)
        {
            // 2시 ~ 5시까지는 어두운 상태 유지
            targetAlpha = maxAlpha;
        }

        // 부드러운 전환
        Color c = nightOverlay.color;
        c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        nightOverlay.color = c;

        // Debug.Log($"Hour: {hour}, Alpha: {c.a}");
    }
}