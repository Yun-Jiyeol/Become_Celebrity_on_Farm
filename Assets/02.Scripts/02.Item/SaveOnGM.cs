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
    }

    private void OnEnable()
    {
        GameManager.Instance.OnActive.Add(gameObject);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnActive.Remove(gameObject);
    }

    public void OnCollider()
    {
        thisCollider.enabled = true;

        if (OffThisCollider)
        {
            if(coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(OffCoroutineCollider());
        }
    }
    public void OffCollider()
    {
        thisCollider.enabled = false;
    }


    IEnumerator OffCoroutineCollider()
    {
        yield return new WaitForSeconds(2f);

        OffCollider();
    }
}
