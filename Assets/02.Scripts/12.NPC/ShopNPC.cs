using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : NPCData, IInteract
{
    public void Interact()
    {
        TestManager.Instance.shopUIManager.ShowShopUI(ShopData);
    }
}
