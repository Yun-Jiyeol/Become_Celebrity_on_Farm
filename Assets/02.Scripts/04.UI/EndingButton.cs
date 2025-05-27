using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class EndingButton : MonoBehaviour
{
    [SerializeField] private Button endButton;
    [SerializeField] private Button nextDayButton;

    [SerializeField] private Image summaryBg;
    [SerializeField] private Image summaryPanel;

    [SerializeField] private TextMeshProUGUI farmNameText;
    [SerializeField] private TextMeshProUGUI watchText;


    void Start()
    {
        endButton.onClick.AddListener(OnClickEndButton);
        nextDayButton.onClick.AddListener(OnNextDayButton);

        summaryBg.gameObject.SetActive(false);
        summaryPanel.gameObject.SetActive(false);

        string farmName = GameManager.Instance.player.GetComponent<PlayerStats>().FarmName;
        farmNameText.text = farmName;
    }


    private void OnEnable()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM(AudioManager.Instance.ReadyAudio["YouBGM"]);
        InvokeRepeating(nameof(RandomViewer), 1f, 2f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(RandomViewer));
        watchText.text = "0";
    }

    void OnClickEndButton()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.bgmSource.volume = 0.05f;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["Button"]);
        SceneChangerManager.Instance.OnClick_LoadScene(SceneChangerManager.Instance.sceneNamesInBuild[0]);
    }

    void OnNextDayButton()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["Button"]);
        summaryPanel.gameObject.SetActive(false);
        StartCoroutine(FadeUI());
    }

    /// <summary>
    /// 정산 패널 활성화
    /// </summary>
    IEnumerator FadeUI()
    {
        summaryBg.gameObject.SetActive(true);
        yield return summaryBg.DOFade(1f, 1.5f)
            .SetUpdate(true)
            .WaitForCompletion();
        summaryPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 하루 지날 때마다 시청자 수 증가
    /// </summary>
    void RandomViewer()
    {
        int date = TimeManager.Instance.currentDay + 1;
        string randomNum = Random.Range(date * 10, date * 15).ToString("N0");
        watchText.text = randomNum;
    }
}