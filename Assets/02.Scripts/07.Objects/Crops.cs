using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : SeedGrow
{
    public bool isDestroyAfterHarvest;
    public int AfterHarvest = 0;

    private void Start()
    {
        HP = 0;
        MaxHP = steps[steps.Count - 1].Hp;

        InvokeRepeating("TestGrow", 3, 3);
    }

    void TestGrow()
    {
        GetDamage(10);
    }

    protected override void calledInteract()
    {
        base.calledInteract();

        ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
        if (isDestroyAfterHarvest)
        {
            GameManager.Instance.CanInteractionObjects["SeededGround"].Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
            GetDamage(-AfterHarvest);
            transform.tag = "Seeded";
        }
    }
}
