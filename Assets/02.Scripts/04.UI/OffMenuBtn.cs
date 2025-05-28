using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OffMenuBtn : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["CreateItem"]);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.OffMenuBtn();
    }
}
