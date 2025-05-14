using UnityEngine;
using UnityEngine.UI;

public class EndingButton : MonoBehaviour
{
    [SerializeField] private Button endButton;
    [SerializeField] private Button nextDayButton;
    Canvas parentCanvas;
    LoadingFader fader;

    void Start()
    {
        endButton.onClick.AddListener(OnClickEndButton);
        nextDayButton.onClick.AddListener(OnNextDayButton);
        
        parentCanvas = GetComponentInParent<Canvas>();
        fader = FindObjectOfType<LoadingFader>();
    }

    void OnClickEndButton()
    {
        //SceneManager.LoadScene("StartScene");
    }

    void OnNextDayButton()
    {
        StartCoroutine(fader.Fade(() =>
        {
            MapManager.Instance.ReloadMap();
        },
        () =>
        {
            parentCanvas.gameObject.SetActive(false);
        }));
    }
}
