using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishingRange
{
    public int ItemData_num;
    public int amount;
}

public class FishingCollider : MonoBehaviour
{
    public FishingRange[] thiscolliderRange;
    private Collider2D collider2D;

    private void Start()
    {
        GameManager.Instance.FishingRange.Add(this);
        collider2D = GetComponent<Collider2D>();
        OffCollider();
    }

    public void OnCollider()
    {
        collider2D.enabled = true;
    }
    public void OffCollider()
    {
        collider2D.enabled = false;
    }
}
