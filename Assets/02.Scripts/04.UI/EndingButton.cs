using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndingButton : MonoBehaviour
{
    [SerializeField] private Button endButton;
    [SerializeField] private Button nextDayButton;

    [SerializeField] private Image expenseBg;
    [SerializeField] private Image expensePanel;
    

    void Start()
    {
        endButton.onClick.AddListener(OnClickEndButton);
        nextDayButton.onClick.AddListener(OnNextDayButton);

        expenseBg.gameObject.SetActive(false);
        expensePanel.gameObject.SetActive(false);
    }

    void OnClickEndButton()
    {
        //SceneManager.LoadScene("StartScene");
    }

    void OnNextDayButton()
    {
        expensePanel.gameObject.SetActive(false);
        StartCoroutine(FadeUI());
    }

    IEnumerator FadeUI()
    {
        expenseBg.gameObject.SetActive(true);
        yield return expenseBg.DOFade(1f, 1.5f)
            .SetUpdate(true)
            .WaitForCompletion();
        expensePanel.gameObject.SetActive(true);
    }
}
