using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public int currentHour = 6;
    public int currentMinute = 0;

    public int currentDay = 0; // 0 = ������
    private readonly string[] weekdays = { "��", "ȭ", "��", "��", "��", "��", "��" };
    public string CurrentWeekday => weekdays[currentDay % 7];

    private bool isSleeping = false;
    private bool isFainted = false;

    private float timeTick = 1f; // 1�ʸ��� ���� �ð� 1�� ���

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
            yield return new WaitForSeconds(timeTick);

            AdvanceTime(1); // 1�� ���
        }
    }

    private void AdvanceTime(int minutes)
    {
        currentMinute += minutes;

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;

            // ����׿� ���
            Debug.Log($"[�ð� ���] {CurrentWeekday} {currentHour}��");

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                Debug.Log($"[���ο� ��!] {CurrentWeekday}����, {currentDay}����");
            }

            // �� �� �ڰ� ���� �ѱ�� ������
            if (currentHour >= 0 && currentHour < 6 && !isSleeping && !isFainted)
            {
                Faint();
            }
        }

        CheckSleep();
    }

    private void CheckSleep()
    {
        // �� 12�� �Ǹ� ������ ���
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
        Debug.Log("[�ڵ� ����] �ʹ� �ʾ����. �ڵ����� ���ϴ�.");
        // TODO: ȸ��, �������� �ѱ��, ���� �ʱ�ȭ ��
    }

    private void Faint()
    {
        isFainted = true;
        Debug.Log("[�Ƿη� ���������ϴ�!] �������� ������...");
        // TODO: �г�Ƽ ����, ���� �̵�, ȸ�� �ð� ��
    }
}
