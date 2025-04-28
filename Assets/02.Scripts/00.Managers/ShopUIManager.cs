using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopUIManager : MonoBehaviour
{
    public ShopUI shopUI;

    public void ShowShopUI()
    {
        shopUI.gameObject.SetActive(true);
        shopUI.StartShopping();
    }

}
