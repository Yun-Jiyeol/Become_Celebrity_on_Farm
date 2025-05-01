using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkNPC : NPCData, IInteract
{
    public List<int> DailyTalk;
    public List<int> RefeatTalk;

    public void Interact()
    {
        TestManager.Instance.shopUIManager.ShowShopUI(ShopData);
    }

    void OpenShopUI()
    {
        TestManager.Instance.gameObject.GetComponent<TextUIManager>().OffTextUI();
        TestManager.Instance.shopUIManager.ShowShopUI(ShopData);
    }
}
