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
        // 플레이어 인벤토리에 넣기 시도
        amount = AddItemToInventory(PlayerHave, getItem, amount);

        // 못 넣은게 남아있으면 → 창고에 넣기 시도
        if (amount > 0)
            amount = InventoryUIManager.Instance.AddItemToWarehouse(getItem, amount);

        // 그래도 남아있으면 → 바닥에 드랍
        if (amount > 0)
            ThrowItem(getItem, amount);

        // UI 새로고침
        InventoryUIManager.Instance.RefreshUI();
    }

    // 인벤토리 추가 공용 로직
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

        return amount; // 못 넣은 갯수 리턴
    }

    public void ThrowItem(ItemDataReader.ItemsData getItem, int amount)
    {
        ItemManager.Instance.spawnItem.DropItem(getItem, amount, transform.position);
    }

    public void TakeItem(ItemDataReader.ItemsData useItem, int amount) //양만큼 가지고 있을 때 실행 시키세요. 퀘스트 용, 제작 용
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

    public bool UseItem(int num, int amount) //1개 씩 사용하게 만들었습니다. 만약 여러개 쓸려면 변경이 필요할 겁니다.
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
