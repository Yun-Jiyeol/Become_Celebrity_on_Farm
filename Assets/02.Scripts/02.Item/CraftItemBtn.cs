using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftItemBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public List<DropItem> needsItems;
    bool canmake = false;

    CraftTooltip crafttool;

    private void Start()
    {
        crafttool = TestManager.Instance.gameObject.GetComponent<CraftManager>().playerCrafting.craftTooltip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        crafttool.gameObject.SetActive(true);
        canmake = crafttool.Setting(needsItems);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        crafttool.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canmake) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
            canmake = crafttool.Setting(needsItems);
        }
    }
}
