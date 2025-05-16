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

    // 아이템 획득 처리
    public void GetItem(ItemDataReader.ItemsData getItem, int amount)
    {
        // 전부 PlayerHave에만 넣음
        amount = AddItemToInventory(PlayerHave, getItem, amount);

        // 못 넣은 게 남아있으면 드롭
        if (amount > 0)
            ThrowItem(getItem, amount);

        InventoryUIManager.Instance.RefreshUI();
    }

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

        return amount;
    }

    public void ThrowItem(ItemDataReader.ItemsData getItem, int amount)
    {
        ItemManager.Instance.spawnItem.DropItem(getItem, amount, transform.position);
    }

    public void TakeItem(ItemDataReader.ItemsData useItem, int amount)
    {
        for (int i = 0; i < PlayerHave.Count; i++)
        {
            if (PlayerHave[i].ItemData_num == useItem.Item_num)
            {
                int takeAmount = Mathf.Min(PlayerHave[i].amount, amount);
                PlayerHave[i].amount -= takeAmount;
                if (PlayerHave[i].amount == 0) PlayerHave[i].ItemData_num = 0;

                amount -= takeAmount;
                if (amount == 0) return;
            }
        }

        InventoryUIManager.Instance.RefreshUI();
    }

    public bool UseItem(int num, int amount)
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

        for (int i = 0; i < PlayerHave.Count; i++)
        {
            if (PlayerHave[i].ItemData_num == num)
            {
                sum += PlayerHave[i].amount;
                if (sum >= amount) return true;
            }
        }

        return false; 
    }
}
