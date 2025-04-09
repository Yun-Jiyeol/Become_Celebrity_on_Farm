using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemFromResource : MonoBehaviour
{
    public string ItemName;
    public int spawnAmount;

    ItemDataReader.ItemsData itemData;

    private void Start()
    {
        itemData = InventoryManager.Instance.itemDataReader.itemsDatas[ItemName];
        InvokeRepeating("DropItem", 0, 3f);
    }

    public void DropItem()
    {
        GameObject go = InventoryManager.Instance.spawnItem.SpawnDropedItems();
        go.GetComponent<DropedItem>().SpawnedDropItem(spawnAmount, gameObject.transform.position, itemData);
    }
}
