using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 슬롯 UI 관리 스크립트 (1칸 단위)
/// </summary>
public class InventorySlotUI : MonoBehaviour
{
    public int slotIndex;         // 몇 번째 슬롯인지 (0 ~ 29)
    public Image itemIcon;        // 아이템 아이콘 이미지
    public TextMeshProUGUI amountText;       // 아이템 수량 텍스트

    private Inventory.Inven invenData;  // 해당 슬롯의 데이터(아이템 정보)

    /// <summary>
    /// 슬롯에 데이터 셋팅 + UI 갱신
    /// </summary>
    public void SetData(Inventory.Inven data)
    {
        invenData = data;

        if (data == null || data.amount <= 0)
        {
            itemIcon.sprite = null;
            amountText.text = "";
            return;
        }

        if (ItemManager.Instance.itemDataReader.itemsDatas.TryGetValue(data.ItemData_num, out var itemData))
        {
            itemIcon.sprite = itemData.Item_sprite;
            amountText.text = data.amount.ToString();
        }
        else
        {
            Debug.LogWarning($"아이템 데이터 못찾음 : {data.ItemData_num}");
            itemIcon.sprite = null;
            amountText.text = "";
        }
    }

    /// <summary>
    /// 현재 슬롯에 있는 데이터 반환
    /// </summary>
    public Inventory.Inven GetData()
    {
        return invenData;
    }
}
