using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance = null;

    public ItemDataReader itemDataReader;
    public SpawnItem spawnItem;
    public SpawnGround spawnGround;
    public ConnectItem connectItem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        itemDataReader = GetComponent<ItemDataReader>();
        spawnItem = GetComponentInChildren<SpawnItem>();
        spawnGround = GetComponentInChildren<SpawnGround>();
        connectItem = GetComponentInChildren<ConnectItem>();
    }

    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
}
