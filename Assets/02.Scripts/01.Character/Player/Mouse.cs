using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!GameManager.Instance.TagOnMouse.Contains(collision.gameObject))
        {
            GameManager.Instance.TagOnMouse.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.TagOnMouse.Remove(collision.gameObject);
    }
}
