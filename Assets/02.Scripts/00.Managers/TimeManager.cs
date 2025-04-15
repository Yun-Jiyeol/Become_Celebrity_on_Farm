using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public int currentHour = 6;
    public int currentMinute = 0;

    public int currentDay = 0; // 0 = 월요일
    private readonly string[] weekdays = { "월", "화", "수", "목", "금", "토", "일" };
    public string CurrentWeekday => weekdays[currentDay % 7];

    private bool isSleeping = false;
    private bool isFainted = false;

    private float timeTick = 1f; // 1초마다 게임 시간 1분 경과

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

            AdvanceTime(1); // 1분 경과
        }
    }

    private void AdvanceTime(int minutes)
    {
        currentMinute += minutes;

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;

            // 디버그용 출력
            Debug.Log($"[시간 경과] {CurrentWeekday} {currentHour}시");

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                Debug.Log($"[새로운 날!] {CurrentWeekday}요일, {currentDay}일차");
            }

            // 잠 안 자고 새벽 넘기면 쓰러짐
            if (currentHour >= 0 && currentHour < 6 && !isSleeping && !isFainted)
            {
                Faint();
            }
        }

        CheckSleep();
    }

    private void CheckSleep()
    {
        // 밤 12시 되면 강제로 잠듦
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
        Debug.Log("[자동 수면] 너무 늦었어요. 자동으로 잠듭니다.");
        // TODO: 회복, 다음날로 넘기기, 상태 초기화 등
    }

    private void Faint()
    {
        isFainted = true;
        Debug.Log("[피로로 쓰러졌습니다!] 무리하지 마세요...");
        // TODO: 패널티 적용, 병원 이동, 회복 시간 등
    }
}
