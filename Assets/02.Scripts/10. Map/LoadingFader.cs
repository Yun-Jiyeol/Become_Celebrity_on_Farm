using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;

public class LoadingFader : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

    Image fader;
    Canvas loadingCanvas;

    void Start()
    {
        DOTween.Init();
        fader = GetComponent<Image>();
        loadingCanvas = GetComponentInParent<Canvas>();

        loadingCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// 맵 전환용
    /// Fade Out, Fade In
    /// </summary>
    public IEnumerator Fade(Action onLoad = null)
    {
        // 0. Fade 동안 카메라 움직임, input 막기
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = false;
            virtualCamera.enabled = false;
        }

        // 1. Fade out 시작
        loadingCanvas.gameObject.SetActive(true);
        yield return fader.DOFade(1f, 1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        // 2. Fade in 전에 해야 하는 것
        onLoad?.Invoke();

        // 3. Fade in 시작
        virtualCamera.enabled = true;
        
        fader.DOFade(0f, 1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        input.enabled = true;
        loadingCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fade Out
    /// </summary>
    public IEnumerator FadeOut(Action onLoad = null)
    {
        // 0. 카메라 / 인풋 막기
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = false;
            virtualCamera.enabled = false;
        }

        // 1. Fade Out
        loadingCanvas.gameObject.SetActive(true);
        yield return fader.DOFade(1f, 1.5f)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        // 2. 해야 할 것 하기
        onLoad?.Invoke();

        // 3. 알파값 돌려놓고 비활성화
        fader.DOFade(0f, 0f);
        loadingCanvas.gameObject.SetActive(false);
    }
}
