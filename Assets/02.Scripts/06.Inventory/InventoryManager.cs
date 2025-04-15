using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("책 페이지들")]
    public List<GameObject> pages; // 책 페이지들 순서대로 넣기

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 책갈피 누르면 호출
    public void SelectPage(int index)
    {
        if (index < 0 || index >= pages.Count)
        {
            Debug.LogWarning("페이지 인덱스 범위 초과");
            return;
        }

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == index);
        }
    }
}


