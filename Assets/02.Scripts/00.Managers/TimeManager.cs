using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public int currentHour = 6;
    public int currentMinute = 0;
    public int currentDay = 0; // 0 = 월요일

    private readonly string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    public string CurrentWeekday => weekdays[currentDay % 7];

    private bool isSleeping = false;
    private bool isFainted = false;

    [Header("Time Settings")]
    [Tooltip("현실 몇 초마다 게임 시간 10분이 흐를지 설정")]
    public float realSecondsPerGameTenMinutes = 10f;

    private float timeTick => realSecondsPerGameTenMinutes;

    private Season season; // Season 스크립트와 연결

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        season = FindObjectOfType<Season>(); // Scene에 Season 스크립트 찾기
    }

    private void Start()
    {
        if (season != null)
        {
            season.SetCurrentDay(currentDay); // Season에 현재 일수를 넘겨줌
        }
        StartCoroutine(TimeFlow());
    }

    private IEnumerator TimeFlow()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeTick); // 현실 10초
            AdvanceTime(10); // 게임 10분 경과

            if (season != null)
            {
                season.UpdateSeason(); // 계절 갱신
            }
        }
    }

    private void AdvanceTime(int minutes)
    {
        currentMinute += minutes;

        if (currentMinute >= 60)
        {
            currentMinute -= 60;
            currentHour++;

            Debug.Log($"[시간 경과] {CurrentWeekday} {currentHour}시 {currentMinute:D2}분");

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                season.SetCurrentDay(currentDay); // Season에 일수 갱신
                //Debug.Log($"[새로운 날!] {CurrentWeekday}요일, {currentDay}일차");
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
        //Debug.Log("[자동 수면] 너무 늦었어요. 자동으로 잠듭니다.");
        // TODO: 회복, 다음날로 넘기기, 상태 초기화 등
    }

    private void Faint()
    {
        isFainted = true;
        //Debug.Log("[피로로 쓰러졌습니다!] 무리하지 마세요...");
        // TODO: 패널티 적용, 병원 이동, 회복 시간 등
    }

    // 외부에서 시간 확인할 수 있도록 프로퍼티 추가 (UI 연동 시 사용하기 편하게)
    public string GetFormattedTime()
    {
        return $"{CurrentWeekday} {currentHour:D2}:{currentMinute:D2}";
    }
}