using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public int currentHour = 6;
    public int currentMinute = 0;
    public int currentDay = 0; // 0 = ������

    private readonly string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    public string CurrentWeekday => weekdays[currentDay % 7];

    private bool isSleeping = false;
    private bool isFainted = false;

    [Header("Time Settings")]
    [Tooltip("���� �� �ʸ��� ���� �ð� 10���� �带�� ����")]
    public float realSecondsPerGameTenMinutes = 10f;

    private float timeTick => realSecondsPerGameTenMinutes;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(TimeFlow());
    }

    private IEnumerator TimeFlow()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeTick); // ���� 10��
            AdvanceTime(10); // ���� 10�� ���
        }
    }

    private void AdvanceTime(int minutes)
    {
        currentMinute += minutes;

        if (currentMinute >= 60)
        {
            currentMinute -= 60;
            currentHour++;

            //Debug.Log($"[�ð� ���] {CurrentWeekday} {currentHour}�� {currentMinute:D2}��");

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                //Debug.Log($"[���ο� ��!] {CurrentWeekday}����, {currentDay}����");
            }

            if (currentHour >= 0 && currentHour < 6 && !isSleeping && !isFainted)
            {
                Faint();
            }
        }

        CheckSleep();
    }

    private void CheckSleep()
    {
        if (currentHour == 24 || (currentHour == 0 && currentMinute == 0))
        {
            if (!isSleeping)
            {
                Sleep();
            }
        }
    }

    private void Sleep()
    {
        isSleeping = true;
        //Debug.Log("[�ڵ� ����] �ʹ� �ʾ����. �ڵ����� ���ϴ�.");
        // TODO: ȸ��, �������� �ѱ��, ���� �ʱ�ȭ ��
    }

    private void Faint()
    {
        isFainted = true;
        //Debug.Log("[�Ƿη� ���������ϴ�!] �������� ������...");
        // TODO: �г�Ƽ ����, ���� �̵�, ȸ�� �ð� ��
    }

    // �ܺο��� �ð� Ȯ���� �� �ֵ��� ������Ƽ �߰� (UI ���� �� ����ϱ� ���ϰ�)
    public string GetFormattedTime()
    {
        return $"{CurrentWeekday} {currentHour:D2}:{currentMinute:D2}";
    }
}