using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SortingOrderGroup : MonoBehaviour
{
    SortingGroup sortingGroup;
    float sortingOrderBase = 5000;
    float distanceBetweenSortingOrders = 10;
    float offset = 0;

    private void Awake()
    {
        sortingGroup = gameObject.GetComponent<SortingGroup>();
        if(sortingGroup == null)
        {
            sortingGroup = gameObject.AddComponent<SortingGroup>();
        }
    }

    private void Start()
    {
        UpdateSortingOrderGroup();
    }

    public void UpdateSortingOrderGroup()
    {
        float newSortingOrder = sortingOrderBase - gameObject.transform.position.y * distanceBetweenSortingOrders + offset;
        sortingGroup.sortingOrder = Mathf.RoundToInt(newSortingOrder);
    }

    public void SetOffset(float newOffSet)
    {
        offset = newOffSet;
    }
}
