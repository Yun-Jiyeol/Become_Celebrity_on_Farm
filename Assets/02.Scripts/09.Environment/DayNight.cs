using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour
{
    [Header("�� ȿ���� UI Image")]
    public Image nightOverlay;

    [Header("��� ���� ����(���İ�)")]
    [Range(0f, 1f)]
    public float maxAlpha = 1f;

    [Tooltip("������ �ٲ�� �ӵ� (�ʴ� ��ȭ��)")]
    public float fadeSpeed = 10f;

    private const int wakeUpHour = 6;       // 6�ú��� ��
    private const int startDarkHour = 17;   // 17�ú��� ��ο���
    private const int darkestHour = 1;      // ���� 1�� ���� ��ο�

    void Start()
    {
        if (nightOverlay != null)
        {
            nightOverlay.color = new Color(0f, 0f, 0f, 0f); // ���� + ����
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
            // ��
            targetAlpha = 0f;
        }
        else if (hour >= startDarkHour && hour <= 23)
        {
            // 17�� ~ 23��: ��ο��� ����
            float t = (hour - startDarkHour) / (float)(darkestHour + 24 - startDarkHour); // 17~1�ñ��� �� 8�ð�
            targetAlpha = Mathf.Lerp(0f, maxAlpha, t);
        }
        else if (hour >= 0 && hour <= darkestHour)
        {
            // 0�� ~ 1��: ��� ������ ����
            float t = (hour + 24 - startDarkHour) / (float)(darkestHour + 24 - startDarkHour);
            targetAlpha = Mathf.Lerp(0f, maxAlpha, t);
        }
        else if (hour > darkestHour && hour < wakeUpHour)
        {
            // 2�� ~ 5�ñ����� ��ο� ���� ����
            targetAlpha = maxAlpha;
        }

        // �ε巯�� ��ȯ
        Color c = nightOverlay.color;
        c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        nightOverlay.color = c;

        // Debug.Log($"Hour: {hour}, Alpha: {c.a}");
    }
}