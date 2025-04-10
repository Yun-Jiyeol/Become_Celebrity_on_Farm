using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemFromResource : MonoBehaviour
{
    public int ItemNum;
    public int spawnAmount;

    private void Start()
    {
        InvokeRepeating("DropItem",0, 3f);
    }

    public void DropItem()
    {
        ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[ItemNum], spawnAmount,gameObject.transform.position);
    }
}
