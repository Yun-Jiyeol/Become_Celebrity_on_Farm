using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemDataReader;

public class Inventory : MonoBehaviour
{
    int inventorySize;

    public List<Inven> PlayerHave;
    [System.Serializable]
    public class Inven
    {
        public ItemDataReader.ItemsData ItemData;
        public int amount;
    }


    private void Start()
    {
        SettingInventorySize();
    }

    public void SettingInventorySize()
    {
        inventorySize = gameObject.GetComponent<Player>().stat.InventorySize;
    }

    public void GetItem(ItemDataReader.ItemsData getItem, int amount)
    {
        if(PlayerHave.Count > 0)
        {
            for (int i = 0; i < PlayerHave.Count; i++)
            {
                Debug.Log(PlayerHave[i].ItemData.Item_name);
                if (PlayerHave[i].ItemData.Item_name == getItem.Item_name)
                {
                    int canadd = getItem.Item_Overlap - PlayerHave[i].amount;
                    if (amount > canadd)
                    {
                        PlayerHave[i].amount += canadd;
                        amount -= canadd;
                    }
                    else
                    {
                        PlayerHave[i].amount += amount;
                        amount = 0;
                    }
                    if (amount <= 0) return;
                }
            }
        }

        while (amount > 0)
        {
            if(PlayerHave.Count == inventorySize)
            {
                ThrowItem(getItem, amount);
                return;
            }

            Inven inven = new Inven();
            inven.ItemData = getItem;

            if (amount > getItem.Item_Overlap)
            {
                amount -= getItem.Item_Overlap;

                inven.amount = getItem.Item_Overlap;
            }
            else
            {
                inven.amount = amount;

                amount = 0;
            }

            PlayerHave.Add(inven);
        }
    }

    public void ThrowItem(ItemDataReader.ItemsData getItem, int amount)
    {
        ItemManager.Instance.spawnItem.DropItem(getItem,amount,gameObject.transform.position);
    }
}
