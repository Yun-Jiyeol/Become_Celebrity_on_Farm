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
        Debug.Log("Triggered with: " + collision.gameObject.tag); // � ������Ʈ�� �浹�ߴ��� Ȯ��

        if (collision.transform.tag == "Player") //�ӽ�
        {
            collision.GetComponent<IInteract>().Interact();
        }
    }
}
