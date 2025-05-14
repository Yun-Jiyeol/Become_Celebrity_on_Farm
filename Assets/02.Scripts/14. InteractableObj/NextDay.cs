using System.Collections;
using UnityEngine;

public class NextDay : MonoBehaviour
{
    [SerializeField] private Canvas endingCanvas;
    [SerializeField] private LoadingFader fader;

    void Start()
    {
        endingCanvas.gameObject.SetActive(false);

        TimeManager.Instance.OnTimeChanged += CheckMidnight;
    }

    /// <summary>
    /// 자정 확인용 함수
    /// </summary>
    void CheckMidnight()
    {
        int curHour = TimeManager.Instance.currentHour;

        // 24시가 된다면 강제 취침
        if (curHour == 24)
            StartCoroutine(ForcedSleep());
    }

    /// <summary>
    /// 다음날로 넘어감
    /// </summary>
    public void Sleep()
    {
        StartCoroutine(fader.FadeOut(() =>
        {
            endingCanvas.gameObject.SetActive(true);
        }));
    }

    /// <summary>
    /// 자정이 될 때까지 안 자면 강제 취침
    /// </summary>
    IEnumerator ForcedSleep()
    {
        Sleep();

        yield return new WaitForSecondsRealtime(1.2f);
        
        MapManager.Instance.MoveMap(MapType.Home);

        yield return new WaitForSecondsRealtime(0.5f);

        // 랜덤 골드 1000~2000(temp) 손해
        int randomGold = Random.Range(10, 20) * 100;
        GoldManager.Instance.SpendGold(randomGold);
    }
}
