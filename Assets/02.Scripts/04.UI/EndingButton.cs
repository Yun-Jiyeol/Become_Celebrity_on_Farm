using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndingButton : MonoBehaviour
{
    [SerializeField] private Button endButton;
    [SerializeField] private Button nextDayButton;

    [SerializeField] private Image summaryBg;
    [SerializeField] private Image summaryPanel;
    

    void Start()
    {
        endButton.onClick.AddListener(OnClickEndButton);
        nextDayButton.onClick.AddListener(OnNextDayButton);

        summaryBg.gameObject.SetActive(false);
        summaryPanel.gameObject.SetActive(false);
    }

    void OnClickEndButton()
    {
        SceneChangerManager.Instance.OnClick_LoadScene(SceneChangerManager.Instance.sceneNamesInBuild[0]);
    }

    void OnNextDayButton()
    {
        summaryPanel.gameObject.SetActive(false);
        StartCoroutine(FadeUI());
    }

    IEnumerator FadeUI()
    {
        summaryBg.gameObject.SetActive(true);
        yield return summaryBg.DOFade(1f, 1.5f)
            .SetUpdate(true)
            .WaitForCompletion();
        summaryPanel.gameObject.SetActive(true);
    }
}
