using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Trees : SeedGrow
{
    public bool isFruitTree;
    public float AdditionalGrow = 0;
    private int MaxAddiitionalGrow;
    private string NowSeasonName;
    private BoxCollider2D treecollider = new BoxCollider2D();
    private bool canGrow;
    public int EndGrow;

    public int WoodItemNum = 1;
    public int WoodItemAmount = 1;
    public string StumpName;
    public float StumpHp;

    private void Awake()
    {
        treecollider = gameObject.GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        base.Start();

        MaxHP = steps[EndGrow].Hp;
        MaxAddiitionalGrow = steps[steps.Count - 1].Hp - (int)MaxHP;
        treecollider.enabled = false;
    }

    public override void Grow(float grow)
    {
        if (!canGrow)
        {
            AdditionalGrow = 0;
            CheckGrow();
            return;
        }

        if (isEndGrow)
        {
            AdditionalGrow = Mathf.Min(MaxAddiitionalGrow, AdditionalGrow + grow);
        }
        GetDamage(grow);
        CheckGrow();
    }

    public override void OnSettingSeason()
    {
        for (int i = 0; i < settingSeason.Count; i++)
        {
            if (settingSeason[i].SeasonType == GameManager.Instance.nowSeason)
            {
                NowSeasonName = settingSeason[i].SeasonName;
                break;
            }
        }

        canGrow = false;

        foreach (Season.SeasonType cangrowseason in canGrowSeason)
        {
            if (cangrowseason == GameManager.Instance.nowSeason)
            {
                canGrow = true;
                break;  
            }
        }
    }

    protected override void CheckGrow()
    {
        string growstep = steps[0].SpriteName;
        bool needChangeOnSeason = false;

        for (int i = 0; i < steps.Count; i++)
        {
            if (HP + AdditionalGrow >= steps[i].Hp)
            {
                if(i >= 2)
                {
                    treecollider.enabled = true;
                }

                WoodItemAmount = Mathf.Min(i + 1, EndGrow + 1);
                growstep = steps[i].SpriteName;
                needChangeOnSeason = steps[i].isChangeOnSeason;
            }
            else
            {
                break;
            }
        }

        if (needChangeOnSeason)
        {
            growstep = NowSeasonName + "_" + growstep;
        }

        Debug.Log(growstep);
        gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[growstep];
    }

    protected override void calledInteract()
    {
        base.calledInteract();

        //플레이어의 도끼? 공격력를 받아와 데미지 계산하는 로직을 추가

        GetDamage(-30);
        if(HP <= 0)
        {
            if (isEndGrow)
            {
                if (AdditionalGrow >= MaxAddiitionalGrow && isFruitTree)
                {
                    ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
                }
                ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[WoodItemNum], WoodItemAmount, gameObject.transform.position);

                GameObject go = new GameObject("TreeStump"); //나무 밑둥 소환술
                go.transform.parent = gameObject.transform;
                go.transform.tag = "Tree";
                go.transform.localScale = Vector3.one;
                go.AddComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[StumpName];
                TreeStump stump = go.AddComponent<TreeStump>();
                stump.Init(StumpHp);
                BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
                collider.offset = new Vector2(0,0.4f);
                collider.size = new Vector2(1, 1);
                GameManager.Instance.SpawnSomething(gameObject.transform.position, go, "TreeGround");

                GameManager.Instance.CanInteractionObjects["TreeGround"].Remove(gameObject);
                Destroy(gameObject);
            }
            else
            {
                ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[WoodItemNum], WoodItemAmount, gameObject.transform.position);
                GameManager.Instance.CanInteractionObjects["TreeGround"].Remove(gameObject);
                Destroy(gameObject);
            }
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
