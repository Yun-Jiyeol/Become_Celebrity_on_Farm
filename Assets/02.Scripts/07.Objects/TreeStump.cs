using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeStump : MonoBehaviour, IHaveHP, IInteract
{
    public float HP { get; set; }
    public float MaxHP { get; set; }

    private int WoodItemNum = 1;
    private int WoodItemAmount = 3;

    public void Init(float _hp)
    {
        HP = _hp;
        MaxHP = _hp;
    }

    public void GetDamage(float amount)
    {
        HP += amount;

        if (HP <= 0)
        {
            ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[WoodItemNum], WoodItemAmount, gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        //플레이어의 도끼? 공격력를 받아와 데미지 계산하는 로직을 추가

        GetDamage(-30);
    }
}
