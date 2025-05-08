using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftItemBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public List<DropItem> needsItems;
    public int AfterItemNum;
    bool canmake = false;
    
    GameObject crafttool;
    CraftTooltip crafttooltip;

    private void Start()
    {
        crafttool = TestManager.Instance.gameObject.GetComponent<CraftManager>().PlayerCraftTable.GetComponent<CraftingScroll>().craftTooltip;
        crafttooltip = crafttool.GetComponent<CraftTooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        crafttool.SetActive(true);
        canmake = crafttooltip.Setting(needsItems);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        crafttool.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canmake) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            foreach(DropItem useitem in needsItems)
            {
                GameManager.Instance.player.GetComponent<Player>().inventory.TakeItem(ItemManager.Instance.itemDataReader.itemsDatas[useitem.SpawnItemNum], useitem.SpawnItemAmount);
            }
            GameManager.Instance.player.GetComponent<Player>().inventory.GetItem(ItemManager.Instance.itemDataReader.itemsDatas[AfterItemNum], 1);
            canmake = crafttooltip.Setting(needsItems);
        }
    }
}
