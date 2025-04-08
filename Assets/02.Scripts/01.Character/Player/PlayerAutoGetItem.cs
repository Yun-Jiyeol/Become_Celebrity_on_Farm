using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoGetItem : MonoBehaviour
{
    CircleCollider2D collider;

    private void Start()
    {
        collider = GetComponent<CircleCollider2D>();

        settingGetItemRange();
    }

    public void settingGetItemRange()
    {
        collider.radius = gameObject.GetComponentInParent<Player>().stat.GetItemRange * 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collider.transform.tag == "Player") //임시
        {
            //아이템이 가지고 있는 GetItem코드 소환
        }
    }
}
