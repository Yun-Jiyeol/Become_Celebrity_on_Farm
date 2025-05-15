using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopHave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    ShopUI _shopUI;
    private int _ItemData_Num = 0;
    private int _amount = 25;
    private int _price = 0;

    public void Setting(ShopUI shotUI, int ItemData_Num, int price)
    {
        gameObject.GetComponent<Shopslot>().amount.text = null;

        _shopUI = shotUI;
        _ItemData_Num = ItemData_Num;
        _price = price;

        gameObject.GetComponent<Shopslot>().Icon.sprite = ItemManager.Instance.itemDataReader.itemsDatas[ItemData_Num].Item_sprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _shopUI.ShopExplain.SetActive(true);
        _shopUI.ShopExplain.GetComponent<ShopExplainPrice>().Setting(_price, _price * _amount);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _shopUI.ShopExplain.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["ShopBuy"]);
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _shopUI.AddBag(_ItemData_Num, 1, _price);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            _shopUI.AddBag(_ItemData_Num, _amount, _price);
        }
    }
}
