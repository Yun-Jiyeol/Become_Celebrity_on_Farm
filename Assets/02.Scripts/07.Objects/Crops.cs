using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : SeedGrow
{
    public bool isDestroyAfterHarvest;
    public int AfterHarvest = 0;
    bool canGrow = false;
    bool OnWater = false;
    int rotsPoint = 0;
    public int Maxrots;

    protected override void Start()
    {
        base.Start();
        OnSettingSeason();
    }

    public override void GetDamage(float amount)
    {
        base.GetDamage(amount);

        if (HP >= MaxHP)
        {
            transform.tag = "EndGrow";
        }
    }

    public override void Grow(float grow)
    {
        if (OnWater)
        {
            OnWater = false;
            GetDamage(grow);
        }
        else
        {
            if (!isEndGrow)
            {
                //썩는다
                rotsPoint++;
                Debug.Log(rotsPoint);
                if (rotsPoint >= Maxrots)
                {
                    DestroyThis();
                }
            }
        }

        CheckGrow();
    }

    public override void OnSettingSeason()
    {
        canGrow = false;

        foreach (Season.SeasonType cangrowseason in canGrowSeason)
        {
            if (cangrowseason == TimeManager.Instance.season.CurrentSeason)
            {
                canGrow = true;
                break;
            }
        }
    }

    protected override void CheckGrow()
    {
        if (!canGrow)
        {
            DestroyThis();
        }

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

        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["Grass"]);
        if (SpawnItemAmount != 0)
        {
            //ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
            var harvestedItem = ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum];
            string harvestedName = harvestedItem.Item_name;

            // 팝업 퀘스트 시스템에 보고
            QuestManager.Instance.ReportProgress(harvestedName, 1);

            // 일일 퀘스트 6일차 수확 보고
            PlannerQuestManager.Instance?.ReportHarvest();

            ItemManager.Instance.spawnItem.DropItem(harvestedItem, SpawnItemAmount, transform.position);
        }

        if (isDestroyAfterHarvest)
        {
            DestroyThis();
        }
        else
        {
            GetDamage(-AfterHarvest);
            CheckGrow();
            isEndGrow = false;
            transform.tag = "Seeded";
        }
    }

    public override void HandInteract()
    {
        base.HandInteract();

        calledInteract();
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Watered")
        {
            Debug.Log("Water");
            OnWater = true;
        }
    }
}
