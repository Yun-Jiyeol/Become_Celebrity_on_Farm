using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject DropedItem;
    List<GameObject> DropedItems = new List<GameObject>();

    private void Start()
    {
        ItemManager.Instance.spawnItem = this;
    }

    public void DropItem(ItemDataReader.ItemsData dropItem, int spawnAmount, Vector3 spawnposition)
    {
        while (spawnAmount > 0)
        {
            int spawninthisturn = spawnAmount;

            if (dropItem.Item_Overlap < spawninthisturn)
            {
                spawninthisturn = dropItem.Item_Overlap;
                spawnAmount -= dropItem.Item_Overlap;
            }
            else
            {
                spawnAmount = 0;
            }

            GameObject go = SpawnDropedItems();
            go.GetComponent<DropedItem>().SpawnedDropItem(spawninthisturn, spawnposition, dropItem);
        }
    }

    GameObject SpawnDropedItems()
    {
        GameObject go = FindOffItems();
        if(go == null)
        {
            go = Instantiate(DropedItem, transform);
            DropedItems.Add(go);
        }

        go.SetActive(true);
        return go;
    }

    GameObject FindOffItems()
    {
        if(DropedItems.Count == 0) return null;

        foreach(GameObject Items in DropedItems)
        {
            if (!Items.activeSelf)
            {
                return Items;
            }
        }
        return null;
    }

    public void OffAllItems()
    {
        foreach (GameObject Items in DropedItems)
        {
            if (Items.activeSelf)
            {
                Items.GetComponent<DropedItem>().offObject();
            }
        }
    }
}
