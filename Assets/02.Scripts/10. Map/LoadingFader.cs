using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingFader : MonoBehaviour
{
    Image fader;

    void Start()
    {
        fader = GetComponent<Image>();
        fader.gameObject.SetActive(false);
    }

    public IEnumerator Fade(System.Action action)
    {
        fader.gameObject.SetActive(true);
        yield return StartCoroutine(FadeOut());

        action?.Invoke();

        yield return StartCoroutine(FadeIn());
        fader.gameObject.SetActive(false);
    }


    /// <summary>
    /// 알파값 0에서 255로
    /// </summary>
    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float duration = 3f;
        Color faderColor = fader.color;

        Time.timeScale = 0f; // !! 코루틴이 안 될 것

        while (elapsed <= duration)
        {
            float t = elapsed / duration;
            faderColor.a = Mathf.Lerp(0f, 1f, t);
            fader.color = faderColor;
            elapsed += Time.unscaledDeltaTime;
        }

        Time.timeScale = 1f;

        yield return null;
    }

    /// <summary>
    /// 알파값 255에서 0으로
    /// </summary>
    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float duration = 3f;

        Color faderColor = fader.color;

        while (elapsed <= duration)
        {
            float t = elapsed / duration;
            faderColor.a = Mathf.Lerp(1f, 0f, t);
            fader.color = faderColor;
            elapsed += Time.unscaledDeltaTime;
        }

        Time.timeScale = 1f;

        yield return null;
    }
}
