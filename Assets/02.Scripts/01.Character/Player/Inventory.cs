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
        public int ItemData_num;
        public int amount;
    }


    private void Start()
    {
        SettingInventorySize();
    }

    public void SettingInventorySize()
    {
        inventorySize = gameObject.GetComponent<Player>().stat.InventorySize;
        
        while(PlayerHave.Count < inventorySize)
        {
            PlayerHave.Add(new Inven { });
        }
    }

    public void GetItem(ItemDataReader.ItemsData getItem, int amount)
    {
        if(PlayerHave.Count > 0) //가지고 있는 아이템에 더 추가될 때
        {
            for (int i = 0; i < PlayerHave.Count; i++)
            {
                if (PlayerHave[i].ItemData_num == getItem.Item_num || PlayerHave[i].ItemData_num == 0)
                {
                    PlayerHave[i].ItemData_num = getItem.Item_num;
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
                    //TestManager.Instance.SettingInven();
                    if (amount <= 0) return;
                }
            }
            ThrowItem(getItem, amount);
        }
    }

    public void ThrowItem(ItemDataReader.ItemsData getItem, int amount)
    {
        ItemManager.Instance.spawnItem.DropItem(getItem,amount,gameObject.transform.position);
    }
}
