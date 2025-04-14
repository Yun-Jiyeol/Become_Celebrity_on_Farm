using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 12;  // �÷��̾� �κ��丮 ũ��
    public List<Inven> PlayerHave;

    [System.Serializable]
    public class Inven
    {
        public int ItemData_num;
        public int amount;
    }

    private void Start()
    {
        SettingInventorySize();
    }

    public void SettingInventorySize()
    {
        while (PlayerHave.Count < inventorySize)
        {
            PlayerHave.Add(new Inven());
        }
    }

    // ������ ȹ�� ó��
    public void GetItem(ItemDataReader.ItemsData getItem, int amount)
    {
        // �÷��̾� �κ��丮�� �ֱ� �õ�
        amount = AddItemToInventory(PlayerHave, getItem, amount);

        // �� ������ ���������� �� â�� �ֱ� �õ�
        if (amount > 0)
            amount = InventoryUIManager.Instance.AddItemToWarehouse(getItem, amount);

        // �׷��� ���������� �� �ٴڿ� ���
        if (amount > 0)
            ThrowItem(getItem, amount);

        // UI ���ΰ�ħ
        InventoryUIManager.Instance.RefreshUI();
    }

    // �κ��丮 �߰� ���� ����
    private int AddItemToInventory(List<Inven> invenList, ItemDataReader.ItemsData getItem, int amount)
    {
        for (int i = 0; i < invenList.Count; i++)
        {
            if (invenList[i].ItemData_num == getItem.Item_num || invenList[i].ItemData_num == 0)
            {
                invenList[i].ItemData_num = getItem.Item_num;

                int canAdd = getItem.Item_Overlap - invenList[i].amount;
                int moveAmount = Mathf.Min(canAdd, amount);

                invenList[i].amount += moveAmount;
                amount -= moveAmount;

                if (amount <= 0)
                    return 0;
            }
        }

        return amount; // �� ���� ���� ����
    }

    public void ThrowItem(ItemDataReader.ItemsData getItem, int amount)
    {
        ItemManager.Instance.spawnItem.DropItem(getItem, amount, transform.position);
    }
}
