using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stones : MonoBehaviour, IHaveHP, IInteract
{
    public float _hp;
    public float HP { get; set; }
    public float MaxHP { get; set; }

    string nowsprite;
    public int SpawnItemNum;
    public int SpawnItemAmount;

    private void Awake()
    {
        HP = _hp;
        MaxHP = _hp;
        nowsprite = gameObject.GetComponent<SpriteRenderer>().sprite.name;
    }

    public void GetDamage(float amount)
    {
        HP += amount;

        if (HP <= 0)
        {
            ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[SpawnItemNum], SpawnItemAmount, gameObject.transform.position);
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
        if (ResourceManager.Instance.splits["Damage_" + nowsprite] != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits["Damage_" + nowsprite];
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[nowsprite];
    }
}
