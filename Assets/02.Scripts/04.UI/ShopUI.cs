using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ShopUIState
{
    Sell,
    Buy
}

public class AmountAndPrice
{
    public int amount;
    public int price;
}

public class ShopUI : MonoBehaviour
{
    public GameObject AllInclude;

    public RectTransform ChooseBtn;
    public RectTransform Shop;
    public RectTransform Middle;
    public RectTransform PlayerInven;
    public GameObject slot;

    [Header("PlayerHave")]
    public GameObject ShopExplain;
    public RectTransform ShopExplainDir;

    public List<ShopPlayerHave> shopPlayerHaves;

    [Header("Middle")]
    public GameObject SlotSpawnPos;
    
    public Dictionary<int, AmountAndPrice> InBag = new Dictionary<int, AmountAndPrice>();
    public List<GameObject> slots;

    public TextMeshProUGUI PlayerHaveGold;
    public TextMeshProUGUI InBagHaveGold;

    [Header("Shop")]
    public GameObject ShopItemSpawnPos;
    public List<GameObject> slotsinshop;

    private ShopUIState nowState = ShopUIState.Buy;

    private void Start()
    {
        UIManager.Instance.shopUIManager.shopUI = this;
        gameObject.SetActive(false);
    }

    public void StartShopping(ShopData _shopData)
    {
        nowState = ShopUIState.Buy;

        SettingShopHave(_shopData);

        AllInclude.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Shop.anchoredPosition = new Vector2(Shop.anchoredPosition.x, 1080);
        PlayerInven.anchoredPosition = new Vector2(PlayerInven.anchoredPosition.x, -1080);
        ChooseBtn.anchoredPosition = new Vector2(180, 200);
        Shop.DOAnchorPos(new Vector2(Shop.anchoredPosition.x, 0), 1f);

        ShopExplain.SetActive(false);
        ShopExplainDir.anchoredPosition = new Vector2(-150, -100);

        ClearBag();
        ChangePlayerGold(0);
    }

    public void ShowShopping()
    {
        nowState = ShopUIState.Buy;

        AllInclude.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        ShopExplainDir.anchoredPosition = new Vector2(-150, -100);

        Shop.DOAnchorPos(new Vector2(Shop.anchoredPosition.x, 0), 1f);
        PlayerInven.DOAnchorPos(new Vector2(PlayerInven.anchoredPosition.x, -1080), 1f);
        ChooseBtn.DOAnchorPos(new Vector2(180, 200), 1f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["ChangeShopUI"]);

        if (InBag.Count > 0)
        {
            foreach(int itemdatanum in InBag.Keys)
            {
                GameManager.Instance.player.GetComponent<Player>().inventory.GetItem(ItemManager.Instance.itemDataReader.itemsDatas[itemdatanum], InBag[itemdatanum].amount);
            }
        }
        ClearBag();
    }
    public void ShowInven()
    {
        nowState = ShopUIState.Sell;
        SettingPlayerHave();

        AllInclude.GetComponent<RectTransform>().DOAnchorPos(new Vector2(960, 0), 0.5f);
        ShopExplainDir.anchoredPosition = new Vector2(150, -100);

        Shop.DOAnchorPos(new Vector2(Shop.anchoredPosition.x, 1080), 1f);
        PlayerInven.DOAnchorPos(new Vector2(PlayerInven.anchoredPosition.x, 0), 1f);
        ChooseBtn.DOAnchorPos(new Vector2(0, 200), 1f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["ChangeShopUI"]);

        ClearBag(true);
    }

    public void ClickOffBtn()
    {
        GameManager.Instance.player.GetComponent<Player>().playerController.isNPCInteract = false;
        foreach(GameObject go in slotsinshop)
        {
            Destroy(go);
        }
        ClearBag();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["ChangeShopUI"]);
        gameObject.SetActive(false);
    }

    void SettingPlayerHave()
    {
        foreach(ShopPlayerHave SPH in shopPlayerHaves)
        {
            if(SPH.IconNum >= GameManager.Instance.player.GetComponent<Player>().inventory.PlayerHave.Count)
            {
                SPH.Setting(0, 0, 0);
            }
            else
            {
                Inventory.Inven inven = GameManager.Instance.player.GetComponent<Player>().inventory.PlayerHave[SPH.IconNum];
                if (inven.ItemData_num == 0)
                {
                    SPH.Setting(0, 0, 0);
                }
                else
                {
                    SPH.Setting(inven.ItemData_num, inven.amount, ItemManager.Instance.itemDataReader.itemsDatas[inven.ItemData_num].Item_Price);
                }
            }
        }
    }

    void SettingShopHave(ShopData _shopData)
    {
        if(_shopData == null || _shopData.sellingCatalogs.Length == 0) return;

        int i = 0;
        for(i = 0; i< _shopData.sellingCatalogs.Length; i++)
        {
            GameObject go = Instantiate(slot, ShopItemSpawnPos.transform);
            go.AddComponent<ShopHave>().Setting(this, _shopData.sellingCatalogs[i].ItemData_num, _shopData.sellingCatalogs[i].Price);
            slotsinshop.Add(go);
        }

        int length = (i % 5 == 0) ? i / 5 : i/ 5 + 1;
        ShopItemSpawnPos.GetComponent<RectTransform>().sizeDelta = new Vector2(0, length * 105);
    }

    public void OnClickClearBtn()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["ClickClearInShop"]);
        ClearBag();
    }

    public void ClearBag(bool isDone = false)
    {
        if(InBag.Count > 0)
        {
            if (nowState == ShopUIState.Sell && !isDone)
            {
                foreach (int itemdatanum in InBag.Keys)
                {
                    GameManager.Instance.player.GetComponent<Player>().inventory.GetItem(ItemManager.Instance.itemDataReader.itemsDatas[itemdatanum], InBag[itemdatanum].amount);
                    SettingPlayerHave();
                }
            }
            InBag.Clear();
        }
        if(slots.Count > 0)
        {
            foreach(GameObject go in slots)
            {
                Destroy(go);
            }
            slots.Clear();
        }

        InBagHaveGold.text = "0000000";
    }

    public void AddBag(int _itemData_num, int _amount, int _price)
    {
        if (InBag.ContainsKey(_itemData_num))
        {
            InBag[_itemData_num].amount += _amount;
        }
        else
        {
            GameObject go = Instantiate(slot, SlotSpawnPos.transform);
            slots.Add(go);
            go.GetComponentInChildren<Shopslot>().Icon.sprite = ItemManager.Instance.itemDataReader.itemsDatas[_itemData_num].Item_sprite;

            AmountAndPrice _AAP = new AmountAndPrice() 
            { 
                amount = _amount,
                price = _price
            };
            InBag.Add(_itemData_num, _AAP);
        }

        SettingGoldBarUI();
    }

    void SettingGoldBarUI()
    {
        int sum = 0;
        if (InBag.Count > 0)
        {
            foreach(AmountAndPrice aap in InBag.Values)
            {
                sum += aap.amount * aap.price;
            }
        }

        InBagHaveGold.text = sum.ToString();
    }

    void ChangePlayerGold(int Changes)
    {
        if(Changes >= 0)
        {
            GoldManager.Instance.AddGold(Changes);
        }
        else
        {
            GoldManager.Instance.SpendGold(-Changes);
        }

        PlayerHaveGold.text = GoldManager.Instance.GetGold().ToString();
    }

    public void OnClickDoneBtn()
    {
        if(InBag.Count == 0) return;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["ClickDoneInShop"]);

        switch (nowState)
        {
            case ShopUIState.Sell:
                ChangePlayerGold(int.Parse(InBagHaveGold.text));
                ClearBag(true);
                break;
            case ShopUIState.Buy:
                if(int.Parse(InBagHaveGold.text) <= GoldManager.Instance.GetGold())
                {
                    PlayerBuyBag();
                    ChangePlayerGold(-int.Parse(InBagHaveGold.text));
                    ClearBag(true);
                }
                break;
        }
    }

    void PlayerBuyBag()
    {
        foreach(int ItemDataNum in InBag.Keys)
        {
            GameManager.Instance.player.GetComponent<Player>().inventory.GetItem(ItemManager.Instance.itemDataReader.itemsDatas[ItemDataNum], InBag[ItemDataNum].amount);
        }
    }
}
