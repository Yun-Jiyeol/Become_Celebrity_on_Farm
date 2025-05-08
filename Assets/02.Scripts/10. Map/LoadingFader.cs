using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;

public class LoadingFader : MonoBehaviour
{
    Image fader;

    [Header("Camera")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        DOTween.Init();

        fader = GetComponent<Image>();
        fader.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fade Out, Fade In
    /// </summary>
    public IEnumerator Fade(Action onLoad = null, Action onAfterFade = null)
    {
        // 0. Fade 동안 카메라 움직임, input 막기
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = false;
            virtualCamera.enabled = false;
        }

        // 1. Fade out 시작
        fader.gameObject.SetActive(true);
        yield return fader.DOFade(1f, 1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        // 2. Fade in 전에 해야 하는 것
        onLoad?.Invoke();

        // 3. Fade in 시작
        virtualCamera.enabled = true;
        yield return fader.DOFade(0f, 1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        // 4. 0.1초 기다리고 input enabled
        yield return new WaitForSecondsRealtime(0.1f);
        input.enabled = true;
        fader.gameObject.SetActive(false);
    }
}
