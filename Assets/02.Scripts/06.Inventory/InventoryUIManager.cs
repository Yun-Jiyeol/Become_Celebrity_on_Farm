using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �κ��丮 ��ü UI ���� ��ũ��Ʈ (30ĭ ���� ����)
/// </summary>
public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    public Inventory playerInventory; // �÷��̾� Inventory ��ũ��Ʈ ����
    public InventorySlotUI[] slots; // ���� UI �迭 (�� 30ĭ)

    public List<Inventory.Inven> warehouseInven = new List<Inventory.Inven>();
    // â��� �κ��丮 ������ (13��° ĭ���� 30��° ĭ���� ���)
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Invoke("RefreshUI", 0.1f);
        //RefreshUI(); // �κ��丮 ���� �� �ʱ�ȭ
    }

    /// <summary>
    /// ��ü �κ��丮 UI ���ΰ�ħ
    /// (�÷��̾� �κ��丮 12ĭ + â�� �κ��丮 18ĭ)
    /// </summary>
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < 12)
            {
                // 0 ~ 11�� ���� �� Player Inventory ����
                slots[i].SetData(playerInventory.PlayerHave[i]);
            }
            else
            {
                // 12 ~ 29�� ���� �� â�� ������ ���
                slots[i].SetData(warehouseInven[i - 12]);
            }
        }
    }

    // ���⿡ ������ Ŭ�� ó��, �̵�, ���� �� �޼��� �߰� ����
    // ��: OnSlotClick(), SwapItem(), MergeItem() ��
}

