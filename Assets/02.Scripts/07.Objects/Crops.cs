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
    }

    public override void Grow(float grow)
    {
        GetDamage(grow);
        CheckGrow();
    }

    public override void OnSettingSeason()
    {

    }

    protected override void CheckGrow()
    {
        string growstep = steps[0].SpriteName;

        for (int i = 0; i < steps.Count; i++)
        {
            if (HP >= steps[i].Hp)
            {
                growstep = steps[i].SpriteName;
            }
            else
            {
                break;
            }
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[growstep];
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
