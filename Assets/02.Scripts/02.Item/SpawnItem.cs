using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject DropedItem;
    List<GameObject> DropedItems = new List<GameObject>();

    private void Start()
    {
        InventoryManager.Instance.spawnItem = this;
    }

    public GameObject SpawnDropedItems()
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
}
