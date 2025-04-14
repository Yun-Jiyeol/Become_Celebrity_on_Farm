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
    }
}


