using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int inventorySize;

    private void Start()
    {
        SettingInventorySize();
    }

    public void SettingInventorySize()
    {
        inventorySize = gameObject.GetComponent<Player>().stat.InventorySize;
    }

    public void GetItem()
    {

    }
}
