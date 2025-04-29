using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPlayerHave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public ShopUI shopui;
    public int IconNum;
    public Image ItemIcon;
    public TextMeshProUGUI Amount;
    private int _ItemData_Num = 0;
    private int _amount = 0;
    private int _price = 0;

    private void Start()
    {
        shopui.shopPlayerHaves.Add(this);
    }

    public void Setting(int ItemData_Num, int amount, int price)
    {
        _ItemData_Num = ItemData_Num;
        _amount = amount;
        _price = price;

        if (ItemData_Num == 0)
        {
            ItemIcon.enabled = false;
            Amount.enabled = false;
        }
        else
        {
            ItemIcon.enabled = true;
            Amount.enabled = true;

            ItemIcon.sprite = ItemManager.Instance.itemDataReader.itemsDatas[ItemData_Num].Item_sprite;
            Amount.text = amount.ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ItemIcon.isActiveAndEnabled) return;
        shopui.ShopExplain.SetActive(true);
        shopui.ShopExplain.GetComponent<ShopExplainPrice>().Setting(_price, _price * _amount);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!ItemIcon.isActiveAndEnabled) return;
        shopui.ShopExplain.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!ItemIcon.isActiveAndEnabled) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameManager.Instance.player.GetComponent<Player>().inventory.UseItem(IconNum,1);
            _amount--;
            shopui.AddBag(_ItemData_Num, 1,_price);

            if( _amount <= 0)
            {
                Setting(0, 0, 0);
                shopui.ShopExplain.SetActive(false);
            }
            else
            {
                Setting(_ItemData_Num, _amount, _price);
                shopui.ShopExplain.GetComponent<ShopExplainPrice>().Setting(_price, _price * _amount);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.Instance.player.GetComponent<Player>().inventory.UseItem(IconNum, _amount);

            shopui.AddBag(_ItemData_Num, _amount, _price);
            Setting(0, 0, 0);
            shopui.ShopExplain.SetActive(false);
        }
    }
}
