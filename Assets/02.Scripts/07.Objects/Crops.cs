using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : SeedGrow
{
    public bool isDestroyAfterHarvest;
    public int AfterHarvest = 0;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("TestGrow", 3, 3);
    }

    void TestGrow()
    {
        GetDamage(10);
        CheckGrow();
    }

    public override void CheckGrow()
    {
        base.CheckGrow();
    }

    protected override void calledInteract()
    {
        if (!isEndGrow) return;

        if(SpawnItemAmount != 0)
        {
            ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
        }
        if (isDestroyAfterHarvest)
        {
            GameManager.Instance.CanInteractionObjects["SeededGround"].Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
            GetDamage(-AfterHarvest);
            CheckGrow();
            transform.tag = "Seeded";
        }
    }

    public override void HandInteract()
    {
        base.HandInteract();

        calledInteract();
    }
}
