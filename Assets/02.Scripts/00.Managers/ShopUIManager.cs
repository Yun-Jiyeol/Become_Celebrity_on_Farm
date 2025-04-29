using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopUIManager : MonoBehaviour
{
    public ShopUI shopUI;

    public void ShowShopUI(ShopData _shopData)
    {
        shopUI.gameObject.SetActive(true);
        shopUI.StartShopping(_shopData);
    }
}
