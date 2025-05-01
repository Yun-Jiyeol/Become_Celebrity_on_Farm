using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LoadingFader : MonoBehaviour
{
    Image fader;

    void Start()
    {
        DOTween.Init();

        fader = GetComponent<Image>();
        fader.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fade Out, Fade In
    /// </summary>
    /// <param name="onLoad"> 맵 로드 </param>
    /// <param name="onAfterFade"> Fader 중복 호출 막기 위한 PlayerInput 제어 </param>
    /// <returns></returns>
    public IEnumerator Fade(Action onLoad, Action onAfterFade = null)
    {
        fader.gameObject.SetActive(true);
        
        yield return fader.DOFade(1f, 1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
        
        onLoad?.Invoke();

        yield return fader.DOFade(0f, 1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        yield return new WaitForSecondsRealtime(0.1f);

        onAfterFade?.Invoke();
        
        fader.gameObject.SetActive(false);
    }
}
