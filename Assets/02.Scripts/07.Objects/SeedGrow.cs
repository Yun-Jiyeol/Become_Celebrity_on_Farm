using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[System.Serializable]
public class StepGrow
{
    public int Hp;
    public string SpriteName;
}

public class SeedGrow : MonoBehaviour, IHaveHP, IInteract
{
    public float HP { get; set; }
    public float MaxHP { get; set; }
    public List<StepGrow> steps;
    public int SpawnItemNum;
    public int SpawnItemAmount;

    bool isEndGrow = false;
    public bool isDestroyAfterHarvest;
    public int AfterHarvest= 0;

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

    public void GetDamage(float amount)
    {
        HP += amount;
        CheckGrow();

        if (HP >= MaxHP)
        {
            HP = MaxHP;
            isEndGrow = true;
            transform.tag = "EndGrow";
        }
    }

    void CheckGrow()
    {
        string growstep = steps[0].SpriteName;

        for (int i = 0; i < steps.Count; i++)
        {
            if(HP >= steps[i].Hp)
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

    public void Interact()
    {
        if(!isEndGrow) return;

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
