using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// �κ��丮 ���� UI ���� ��ũ��Ʈ (1ĭ ����)
public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotIndex;                    // �� ��° �������� (0 ~ 29)
    public Image itemIcon;                   // ������ ������ �̹���
    public TextMeshProUGUI amountText;       // ������ ���� �ؽ�Ʈ

    private Inventory.Inven invenData;  // �ش� ������ ������

    public GameObject chooseIndicator; // Choose ������Ʈ

    // ���Կ� ������ ���� + UI ����
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
            Debug.LogWarning($"������ ������ ��ã�� : {data.ItemData_num}");
            itemIcon.sprite = null;
            amountText.text = "";
        }
    }

    // ���� ���Կ� �ִ� ������ ��ȯ
    public Inventory.Inven GetData()
    {
        return invenData;
    }
    public void OnClickSlot()
    {
        InventoryUIManager.Instance.OnSlotClick(this);
    }

    // ���콺 �ö��� �� Tooltip ǥ�� 
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (invenData != null && invenData.amount > 0)
        {
            if (ItemManager.Instance.itemDataReader.itemsDatas.TryGetValue(invenData.ItemData_num, out var itemData))
            {
                TooltipManager.Instance.ShowTooltip(itemData, Input.mousePosition);
            }
        }
    }

    // ���콺 ���������� �� Tooltip �����
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
    public void SetSelected(bool selected)
    {
        if (chooseIndicator != null)
            chooseIndicator.SetActive(selected);
    }
}
