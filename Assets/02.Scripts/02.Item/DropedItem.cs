using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedItem : MonoBehaviour, IInteract
{
    //아이템 데이터
    Vector3 startPosition;
    public Rigidbody2D rig;
    public int amount; //갯수

    public void SpawnedDropItem(int _amunt, Vector3 _position)//+ 아이템 데이터
    {
        amount = _amunt;
        startPosition = _position;
        transform.position = _position;

        rig.gravityScale = 1;
        StartCoroutine(DropItem());
    }

    IEnumerator DropItem()
    {
        float Angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle));
        Vector2 force = dir * 5f; //방향 설정 후 랜덤 힘만큼 쏜다

        rig.AddForce(force,ForceMode2D.Impulse); //힘을 준다

        while (Vector2.Distance(startPosition,transform.position) < 2f)
        {
            yield return null;
        }

        rig.gravityScale = 0;
        rig.velocity = Vector2.zero;
    }



    public void Interact()
    {

    }
}
