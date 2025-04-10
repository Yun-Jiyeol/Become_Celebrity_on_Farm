using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemFromResource : MonoBehaviour
{
    public string ItemName;
    public int spawnAmount;

    private void Start()
    {
        Invoke("DropItem", 3f);
    }

    public void DropItem()
    {
        ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[ItemName], spawnAmount,gameObject.transform.position);
    }
}
