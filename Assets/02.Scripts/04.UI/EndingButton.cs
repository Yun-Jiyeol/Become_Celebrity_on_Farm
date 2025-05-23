using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingButton : MonoBehaviour
{
    [SerializeField] private Button endButton;
    [SerializeField] private Button nextDayButton;

    [SerializeField] private Image summaryBg;
    [SerializeField] private Image summaryPanel;

    [SerializeField] private TextMeshProUGUI farmNameText;



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
}
