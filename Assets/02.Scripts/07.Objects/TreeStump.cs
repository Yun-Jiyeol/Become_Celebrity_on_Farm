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
        //�÷��̾��� ����? ���ݷ¸� �޾ƿ� ������ ����ϴ� ������ �߰�

        GetDamage(-30);
    }
}
