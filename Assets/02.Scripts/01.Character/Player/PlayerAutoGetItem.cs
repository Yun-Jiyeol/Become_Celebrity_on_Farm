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
        Debug.Log("Triggered with: " + collision.gameObject.tag); // 어떤 오브젝트와 충돌했는지 확인

        if (collision.transform.tag == "Player") //임시
        {
            collision.GetComponent<IInteract>().Interact();
        }
    }
}
