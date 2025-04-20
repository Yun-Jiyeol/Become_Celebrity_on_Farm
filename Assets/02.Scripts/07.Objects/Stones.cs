using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stones : MonoBehaviour, IHaveHP, IInteract
{
    public float _hp;
    public float HP { get; set; }
    public float MaxHP { get; set; }

    public int SpawnItemNum;
    public int SpawnItemAmount;

    private void Awake()
    {
        HP = _hp;
        MaxHP = _hp;
    }

    public void GetDamage(float amount)
    {
        HP += amount;

        if (HP <= 0)
        {
            ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
            //GameManager.Instance.CanInteractionObjects["TreeGround"].Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        GetDamage(-20);
    }
}
