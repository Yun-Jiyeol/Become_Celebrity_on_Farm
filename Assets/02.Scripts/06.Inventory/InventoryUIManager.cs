using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ItemDataReader;


// 인벤토리 전체 UI 관리 스크립트 (30칸 전부 관리)
public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;
    public Inventory playerInventory; // 플레이어 Inventory 스크립트 참조
    public RectTransform invenRect;
    public Transform playerTransform;

    // 슬롯 UI 배열 (총 30칸)
    public InventorySlotUI[] slots;

    // 창고용 인벤토리 데이터 (13번째 칸부터 30번째 칸까지 사용)
    public List<Inventory.Inven> warehouseInven = new List<Inventory.Inven>();

    public InventorySlotUI selectedSlot;

    public GameObject mouseFollowItemObj;      // 마우스 따라다닐 UI
    public Image mouseFollowIcon;              // 아이콘 이미지
    public TextMeshProUGUI mouseFollowAmount;  // 개수 표시

    // 임시 보관
    private int tempItemData_num;  
    private int tempItemAmount;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        while (warehouseInven.Count < 18)
            warehouseInven.Add(new Inventory.Inven());

        //Invoke("RefreshUI", 0.1f); // 인벤토리 열릴 때 초기화
        RefreshUI();
    }
    private void Update()
    {
        if (mouseFollowItemObj.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            mouseFollowItemObj.transform.position = mousePos + new Vector3(5, -5, 0); // 마우스 오른쪽 아래
        }

        // 아이템 들고 있는 상태 + 마우스 왼클릭
        if (selectedSlot != null && Input.GetMouseButtonDown(0))
        {
            // 인벤토리 RectTransform 안에 마우스 있는지 검사
            if (!RectTransformUtility.RectangleContainsScreenPoint(invenRect, Input.mousePosition))
            {
                // 밖에서 눌렀으니까 아이템 드롭
                DropSelectedItem();
            }
        }
    }

    // 전체 인벤토리 UI 새로고침
    // (플레이어 인벤토리 12칸 + 창고 인벤토리 18칸)
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < 12)
                slots[i].SetData(playerInventory.PlayerHave[i]);
            else
                slots[i].SetData(warehouseInven[i - 12]);
        }

    }

    public void OnSlotClick(InventorySlotUI clickedSlot)
    {
        // 첫 클릭 → 아이템 들기
        if (selectedSlot == null)
        {
            var data = clickedSlot.GetData();
            if (data == null || data.amount <= 0)
                return;

            selectedSlot = clickedSlot;

            // 마우스 따라다닐 아이템 세팅
            var itemData = ItemManager.Instance.itemDataReader.itemsDatas[data.ItemData_num];
            mouseFollowIcon.sprite = itemData.Item_sprite;
            mouseFollowAmount.text = data.amount.ToString();
            mouseFollowItemObj.SetActive(true);

            // 선택 슬롯 데이터 따로 보관
            tempItemData_num = data.ItemData_num;
            tempItemAmount = data.amount;

            // 슬롯 비우기
            data.ItemData_num = 0;
            data.amount = 0;

            RefreshUI();
            return;
        }

        // 두번째 클릭
        bool success = SwapOrMergeItem(selectedSlot, clickedSlot);

        if (!success)
        {
            // 못넣었으면 원래 자리로 복구
            selectedSlot.GetData().ItemData_num = tempItemData_num;
            selectedSlot.GetData().amount = tempItemAmount;
        }

        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);
        RefreshUI();
    }
    public void OnClickTrashButton()
    {
        if (selectedSlot == null || tempItemAmount <= 0)
        {
            Debug.Log("삭제할 아이템이 없음.");
            return;
        }

        // 그냥 들고 있는 데이터만 날림
        tempItemData_num = 0;
        tempItemAmount = 0;

        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);

        RefreshUI();
    }
    private bool SwapOrMergeItem(InventorySlotUI from, InventorySlotUI to)
    {
        var fromData = from.GetData();
        var toData = to.GetData();

        // 같은 아이템 = Merge
        if (toData.ItemData_num == tempItemData_num && tempItemData_num != 0)
        {
            var itemData = ItemManager.Instance.itemDataReader.itemsDatas[tempItemData_num];

            int canAdd = itemData.Item_Overlap - toData.amount;
            int moveAmount = Mathf.Min(canAdd, tempItemAmount);

            toData.amount += moveAmount;
            tempItemAmount -= moveAmount;

            return tempItemAmount <= 0;
        }
        // 빈 슬롯 = 그냥 넣기
        else if (toData.amount <= 0)
        {
            toData.ItemData_num = tempItemData_num;
            toData.amount = tempItemAmount;
            return true;
        }
        // 다른 아이템 = 자리 Swap
        else
        {
            // Swap 처리
            int tempNum = toData.ItemData_num;
            int tempAmount = toData.amount;

            // toData = 들고있던거 넣기
            toData.ItemData_num = tempItemData_num;
            toData.amount = tempItemAmount;

            // fromData = 원래 toData 데이터 넣기
            fromData.ItemData_num = tempNum;
            fromData.amount = tempAmount;

            return true;
        }
    }

    // 창고 인벤토리에 아이템 추가
    public int AddItemToWarehouse(ItemDataReader.ItemsData getItem, int amount)
    {
        return AddItemToInventory(warehouseInven, getItem, amount);
    }

    // 공용 아이템 추가 로직
    private int AddItemToInventory(List<Inventory.Inven> invenList, ItemDataReader.ItemsData getItem, int amount)
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
    private void DropSelectedItem()
    {
        if (tempItemData_num == 0 || tempItemAmount <= 0)
        {
            Debug.Log("들고 있는 아이템 정보 없음");
            return;
        }

        var itemData = ItemManager.Instance.itemDataReader.itemsDatas[tempItemData_num];
        Debug.Log($"드랍 시도: {itemData.Item_name}, 수량: {tempItemAmount}");

        // 드랍
        ItemManager.Instance.spawnItem.DropItem(itemData, tempItemAmount, playerTransform.position);

        // 초기화
        tempItemData_num = 0;
        tempItemAmount = 0;
        selectedSlot = null;
        mouseFollowItemObj.SetActive(false);

        RefreshUI();
    }
}

