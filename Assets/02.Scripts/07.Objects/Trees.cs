using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : SeedGrow
{
    public bool isFruitTree;
    public int AdditionalGrow = 0;
    private int MaxAddiitionalGrow;
    public int EndGrow;

    public int WoodItemNum = 1;
    public int WoodItemAmount = 1;
    protected override void Start()
    {
        base.Start();
        InvokeRepeating("TestGrow", 3, 3);

        MaxHP = steps[EndGrow].Hp;
        MaxAddiitionalGrow = steps[steps.Count - 1].Hp - (int)MaxHP;
    }

    void TestGrow()
    {
        if (isEndGrow)
        {
            AdditionalGrow = Mathf.Min(MaxAddiitionalGrow, AdditionalGrow + 10);
        }
        GetDamage(10);
        CheckGrow();
    }

    public override void CheckGrow()
    {
        string growstep = steps[0].SpriteName;

        for (int i = 0; i < steps.Count; i++)
        {
            if (HP + AdditionalGrow >= steps[i].Hp)
            {
                WoodItemAmount = Mathf.Min(i + 1, EndGrow + 1);
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
        base.calledInteract();

        //플레이어의 도끼? 공격력를 받아와 데미지 계산하는 로직을 추가

        GetDamage(-30);
        if(HP <= 0)
        {
            if (AdditionalGrow >= MaxAddiitionalGrow)
            {
                ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
            }
            ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[WoodItemNum], WoodItemAmount, gameObject.transform.position);
            GameManager.Instance.CanInteractionObjects["TreeGround"].Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public override void HandInteract()
    {
        base.HandInteract();

        if (!isFruitTree) return;

        if (AdditionalGrow >= MaxAddiitionalGrow)
        {
            ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
            AdditionalGrow = 0;
            CheckGrow();
        }
    }
}
