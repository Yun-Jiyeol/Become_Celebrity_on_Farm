using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("å ��������")]
    public List<GameObject> pages; // å �������� ������� �ֱ�

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // å���� ������ ȣ��
    public void SelectPage(int index)
    {
        if (index < 0 || index >= pages.Count)
        {
            Debug.LogWarning("������ �ε��� ���� �ʰ�");
            return;
        }

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == index);
        }

        // ������ 3��(index == 3)�̸� SlotText�� Trash ��Ȱ��ȭ
        bool hideExtras = (index == 3);

        // �κ��丮 UI ���ο��� ������Ʈ ã��
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
            Debug.LogWarning("InventoryUI ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}


