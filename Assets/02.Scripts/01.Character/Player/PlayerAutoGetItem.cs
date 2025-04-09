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
        if (collision.transform.tag == "Player") //юс╫ц
        {
            collision.GetComponent<IInteract>().Interact();
        }
    }
}
