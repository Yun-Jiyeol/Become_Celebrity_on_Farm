using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
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
        while (PlayerHave.Count < GetComponent<Player>().stat.InventorySize)
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

    public void TakeItem(ItemDataReader.ItemsData useItem, int amount) //�縸ŭ ������ ���� �� ���� ��Ű����. ����Ʈ ��, ���� ��
    {
        for(int i =0; i < PlayerHave.Count; i++)
        {
            if(PlayerHave[i].ItemData_num == useItem.Item_num)
            {
                int Takeamount = Mathf.Min(PlayerHave[i].amount, amount);
                PlayerHave[i].amount -= Takeamount;
                if (PlayerHave[i].amount == 0) PlayerHave[i].ItemData_num = 0;

                amount -= Takeamount;
                if(amount == 0) return;
            }
        }
        InventoryUIManager.Instance.RefreshUI();
    }

    public bool UseItem(int num, int amount) //1�� �� ����ϰ� ��������ϴ�. ���� ������ ������ ������ �ʿ��� �̴ϴ�.
    {
        if (PlayerHave[num].amount < amount) return false;

        PlayerHave[num].amount -= amount;
        if (PlayerHave[num].amount == 0) PlayerHave[num].ItemData_num = 0;
        InventoryUIManager.Instance.RefreshUI();
        return true;
    }

    public bool FindItem(int num, int amount)
    {
        int sum = 0;

        for(int i =0; i< PlayerHave.Count; i++)
        {
            if (PlayerHave[i].ItemData_num == num)
            {
                sum += PlayerHave[i].amount;
                if(sum >= amount) return true;
            }
        }
        return false;
    }
}
