using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    public int SpawnItemNum;
    public int SpawnItemAmount;
}

public class Stones : MonoBehaviour, IHaveHP, IInteract
{
    public float _hp;
    public float HP { get; set; }
    public float MaxHP { get; set; }

    public string Itemspritename;
    public string NowObjectname;
    string nowsprite;

    public DropItem[] dropitems;

    private void Awake()
    {
        HP = _hp;
        MaxHP = _hp;
        nowsprite = Itemspritename + NowObjectname; 
    }

    public void GetDamage(float amount)
    {
        HP += amount;

        if (HP <= 0)
        {
            for(int i =0; i < dropitems.Length; i++)
            {
                ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[dropitems[i].SpawnItemNum], dropitems[i].SpawnItemAmount, gameObject.transform.position);
            }
            Destroy(gameObject);
        }
        else if (amount <= 0)
        {
            StartCoroutine(DamageCoroutine());
        }
    }

    public void Interact()
    {
        GetDamage(-10);
    }
    IEnumerator DamageCoroutine()
    {
        if (ResourceManager.Instance.splits["Damage_" + NowObjectname] != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits["Damage_" + NowObjectname];
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[nowsprite];
    }
}
