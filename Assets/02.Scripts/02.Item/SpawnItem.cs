using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : ObjectPolling
{
    public GameObject DropedItem;

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

            GameObject go = SpawnOrFindThings();
            go.GetComponent<DropedItem>().SpawnedDropItem(spawninthisturn, spawnposition, dropItem);
        }
    }

    protected override GameObject SpawnOrFindThings()
    {
        GameObject go = FindOffthings();
        if(go == null)
        {
            go = Instantiate(DropedItem, transform);
            Things.Add(go);
        }

        go.SetActive(true);
        return go;
    }
}
