using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveOnGM : MonoBehaviour
{
    Coroutine coroutine;
    public bool OffThisCollider;
    public Collider2D thisCollider;

    private void Start()
    {
        thisCollider = GetComponent<Collider2D>();
        GameManager.Instance.OnActive.Add(gameObject);
    }

    public void OnCollider()
    {
        thisCollider.enabled = true;
    }
    public void OffCollider()
    {
        if (OffThisCollider)
        {
            thisCollider.enabled = false;
        }
    }
}
