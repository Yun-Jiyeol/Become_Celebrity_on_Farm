using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OffGameBtn : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["CreateItem"]);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SceneChangerManager.Instance.OnClick_QuitGame();
    }
}
