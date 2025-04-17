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

    protected bool isEndGrow = false;

    protected virtual void Start()
    {
        HP = 0;
        MaxHP = steps[steps.Count - 1].Hp;
    }


    public void GetDamage(float amount)
    {
        HP += amount;

        if (HP >= MaxHP)
        {
            HP = MaxHP;
            isEndGrow = true;
            transform.tag = "EndGrow";
        }
    }

    public virtual void CheckGrow()
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

    public virtual void Grow(float grow)
    {

    }

    public void Interact()
    {
        calledInteract();
    }

    protected virtual void calledInteract()
    {

    }

    public virtual void HandInteract()
    {
        if (!isEndGrow) return;
    }
}
