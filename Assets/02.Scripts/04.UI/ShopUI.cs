using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopUIState
{
    Sell,
    Buy
}

public class ShopUI : MonoBehaviour
{
    public GameObject AllInclude;

    public RectTransform Shop;
    public RectTransform Middle;
    public RectTransform PlayerInven;

    private ShopUIState nowState = ShopUIState.Buy;

    private void Start()
    {
        TestManager.Instance.shopUIManager.shopUI = this;
        //gameObject.SetActive(false);
        StartShopping();
    }

    public void StartShopping()
    {
        nowState = ShopUIState.Buy;

        AllInclude.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Shop.anchoredPosition = new Vector2(Shop.anchoredPosition.x, 1080);
        PlayerInven.anchoredPosition = new Vector2(PlayerInven.anchoredPosition.x, -1080);
        Shop.DOAnchorPos(new Vector2(Shop.anchoredPosition.x, 0), 1f);
    }

    public void ShowShopping()
    {
        nowState = ShopUIState.Buy;

        AllInclude.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);

        Shop.DOAnchorPos(new Vector2(Shop.anchoredPosition.x, 0), 1f);
        PlayerInven.DOAnchorPos(new Vector2(PlayerInven.anchoredPosition.x, -1080), 1f);
    }
    public void ShowInven()
    {
        nowState = ShopUIState.Sell;

        AllInclude.GetComponent<RectTransform>().DOAnchorPos(new Vector2(960, 0), 0.5f);

        Shop.DOAnchorPos(new Vector2(Shop.anchoredPosition.x, 1080), 1f);
        PlayerInven.DOAnchorPos(new Vector2(PlayerInven.anchoredPosition.x, 0), 1f);
    }
}
