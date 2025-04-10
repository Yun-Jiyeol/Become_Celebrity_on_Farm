using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int inventorySize;
    public List<ItemDataReader.ItemsData> PlayerHave;

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

    }

    public void ThrowItem()
    {

    }
}
