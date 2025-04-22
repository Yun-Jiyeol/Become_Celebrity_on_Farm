using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] public SpriteRenderer backgroundRenderer; // �ϴ� ��� SpriteRenderer
    public Color dayColor = new Color(0.8f, 0.9f, 1f);  // ���� �ϴû�
    public Color nightColor = new Color(0.1f, 0.1f, 0.2f); // ��ο� ����

    [Range(0, 24)]
    public int dayStartHour = 6;   // ��ħ 6��
    [Range(0, 24)]
    public int nightStartHour = 18; // ���� 6��

    private void Update()
    {
        if (TimeManager.Instance == null || backgroundRenderer == null)
            return;

        int currentHour = TimeManager.Instance.currentHour;

        // 0~1 ������ ������ ���
        float t = 0f;
        if (currentHour >= dayStartHour && currentHour < nightStartHour)
        {
            // �� (dayStartHour ~ nightStartHour)
            t = Mathf.InverseLerp(dayStartHour, nightStartHour, currentHour);
            backgroundRenderer.color = Color.Lerp(dayColor, nightColor, t);
        }
        else
        {
            // �� (nightStartHour ~ ������ dayStartHour)
            int hour = currentHour;
            if (currentHour < dayStartHour) hour += 24; // ���� �ð� ����

            t = Mathf.InverseLerp(nightStartHour, dayStartHour + 24, hour);
            backgroundRenderer.color = Color.Lerp(nightColor, dayColor, t);
        }
    }
}

