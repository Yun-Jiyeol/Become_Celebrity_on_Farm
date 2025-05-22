using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartSceneBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject MouseOn;
    public GameObject MouseOff;

    private void Start()
    {
        MouseOn.SetActive(false);
        MouseOff.SetActive(true);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["Grass"]);
        MouseOn.SetActive(true);
        MouseOff.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseOn.SetActive(false);
        MouseOff.SetActive(true);
    }
}
