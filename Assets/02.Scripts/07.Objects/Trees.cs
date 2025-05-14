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
    private bool canGrow;
    string growstep;
    string seasonstep;
    public int EndGrow;

    public int WoodItemNum = 1;
    public int WoodItemAmount = 1;
    public string StumpName;
    public float StumpHp;

    protected override void Start()
    {
        base.Start();

        MaxHP = steps[EndGrow].Hp;
        MaxAddiitionalGrow = steps[steps.Count - 1].Hp - (int)MaxHP;
        OnSettingSeason();
        CheckGrow();
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
            HP = steps[EndGrow].Hp;
            AdditionalGrow = Mathf.Min(MaxAddiitionalGrow, AdditionalGrow + grow);
        }
        GetDamage(grow);
        CheckGrow();
    }

    public override void OnSettingSeason()
    {
        for (int i = 0; i < settingSeason.Count; i++)
        {
            if (settingSeason[i].SeasonType == TimeManager.Instance.season.CurrentSeason)
            {
                NowSeasonName = settingSeason[i].SeasonName;
                break;
            }
        }

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
        growstep = steps[0].SpriteName;
        bool needChangeOnSeason = false;

        for (int i = 0; i < steps.Count; i++)
        {
            if (HP + AdditionalGrow >= steps[i].Hp)
            {
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
            seasonstep = NowSeasonName + "_" + growstep;
        }
        else
        {
            seasonstep =  growstep;
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[seasonstep];
    }

    protected override void calledInteract()
    {
        base.calledInteract();

        //플레이어의 도끼? 공격력를 받아와 데미지 계산하는 로직을 추가

        GetDamage(-(GameManager.Instance.player.GetComponent<Player>().stat.Attack + GameManager.Instance.player.GetComponent<Player>().playerController.ItemDamage));
        if (HP <= 0)
        {
            if (isEndGrow)
            {
                if (AdditionalGrow >= MaxAddiitionalGrow && isFruitTree)
                {
                    ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
                }
                ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[WoodItemNum], WoodItemAmount, gameObject.transform.position);

                GameObject go = new GameObject("TreeStump"); //나무 밑둥 소환술
                go.transform.parent = GameManager.Instance.transform;
                go.transform.position = gameObject.transform.position;
                go.transform.tag = "Tree";
                go.transform.localScale = Vector3.one;
                go.AddComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[StumpName];
                BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
                collider.offset = new Vector2(0, 0.4f);
                collider.size = new Vector2(1, 1);
                TreeStump stump = go.AddComponent<TreeStump>();
                stump.GetDamage(StumpHp);
                go.AddComponent<SortingOrderGroup>();
            }
            else
            {
                ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[WoodItemNum], WoodItemAmount, gameObject.transform.position);
            }
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DamageCoroutine());
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

    IEnumerator DamageCoroutine()
    {
        if(ResourceManager.Instance.splits["Damage_" + growstep] != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits["Damage_" + growstep];
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[seasonstep];
    }
}
