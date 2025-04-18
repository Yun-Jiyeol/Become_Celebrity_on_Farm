using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[System.Serializable]
public class StepGrow
{
    public int Hp;
    public bool isChangeOnSeason;
    public string SpriteName;
}

[System.Serializable]
public class SeedGrowOnSeason
{
    public Season.SeasonType SeasonType;
    public string SeasonName;
}

public class SeedGrow : MonoBehaviour, IHaveHP, IInteract
{
    public float HP { get; set; }
    public float MaxHP { get; set; }

    public List<StepGrow> steps;
    public List<SeedGrowOnSeason> settingSeason;
    public List<Season.SeasonType> canGrowSeason;
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

    public virtual void OnSettingSeason()
    {

    }

    protected virtual void CheckGrow()
    {

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
