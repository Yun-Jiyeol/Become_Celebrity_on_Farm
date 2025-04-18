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

        // 페이지 3번(index == 3)이면 SlotText랑 Trash 비활성화
        bool hideExtras = (index == 3);

        // 인벤토리 UI 내부에서 오브젝트 찾기
        GameObject inventoryUI = GameObject.Find("InventoryUI");

        if (inventoryUI != null)
        {
            Transform itemslotText = inventoryUI.transform.Find("ItemSlotTxt");
            Transform trash = inventoryUI.transform.Find("Trash");

            if (itemslotText != null) itemslotText.gameObject.SetActive(!hideExtras);
            if (trash != null) trash.gameObject.SetActive(!hideExtras);
        }
        else
        {
            Debug.LogWarning("InventoryUI 오브젝트를 찾을 수 없습니다.");
        }
    }
}


