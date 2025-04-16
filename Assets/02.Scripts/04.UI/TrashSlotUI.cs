using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image trashImage;
    public Sprite closedSprite;
    public Sprite openSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        trashImage.sprite = openSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        trashImage.sprite = closedSprite;
    }
}

