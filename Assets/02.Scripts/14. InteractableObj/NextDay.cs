using System.Collections;
using UnityEngine;

public class NextDay : MonoBehaviour
{
    [SerializeField] private Canvas endingCanvas;
    [SerializeField] private LoadingFader fader;

    public bool isForced = false;

    void Start()
    {
        endingCanvas.gameObject.SetActive(false);

        TimeManager.Instance.OnDayChanged += StartForcedSleep;
    }

    void StartForcedSleep()
    {
        if (!TimeManager.Instance.isSleeping)
            StartCoroutine(ForcedSleep());
    }

    /// <summary>
    /// 다음날로 넘어감
    /// </summary>
    public void Sleep()
    {
        StartCoroutine(fader.FadeOut(() =>
        {
            TimeManager.Instance.isSleeping = true;
            endingCanvas.gameObject.SetActive(true);
        }));
    }

    /// <summary>
    /// 자정이 될 때까지 안 자면 강제 취침
    /// </summary>
    IEnumerator ForcedSleep()
    {
        isForced = true;
        Sleep();

        yield return new WaitForSecondsRealtime(1.3f);

        MapManager.Instance.MoveMap(MapType.Home);

        yield return new WaitForSecondsRealtime(0.5f);

        // 랜덤 골드 1000~2000(temp) 손해
        int randomGold = Random.Range(10, 20) * 100;
        GoldManager.Instance.SpendGold(randomGold);
    }
}
