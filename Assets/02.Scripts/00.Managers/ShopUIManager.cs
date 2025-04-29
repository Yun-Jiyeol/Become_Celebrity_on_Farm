using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopUIManager : MonoBehaviour
{
    public ShopUI shopUI;
    public ShopData shopData;

    private void Start()
    {
        Invoke("LateStart", 0.5f);
    }

    void LateStart()
    {
        ShowShopUI(shopData);
    }

    public void ShowShopUI(ShopData _shopData)
    {
        shopUI.gameObject.SetActive(true);
        shopUI.StartShopping(_shopData);
    }
}
