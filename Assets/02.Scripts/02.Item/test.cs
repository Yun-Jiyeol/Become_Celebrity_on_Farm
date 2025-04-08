using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject DropItem;

    private void Start()
    {
        InvokeRepeating("SpawnDropItem", 0, 2f);
    }

    void SpawnDropItem()
    {
        GameObject go = Instantiate(DropItem);
        go.GetComponent<DropedItem>().SpawnedDropItem(1, gameObject.transform.position);
    }
}
