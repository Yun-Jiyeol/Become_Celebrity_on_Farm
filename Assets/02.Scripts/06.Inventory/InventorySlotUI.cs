using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �κ��丮 ���� UI ���� ��ũ��Ʈ (1ĭ ����)
public class InventorySlotUI : MonoBehaviour
{
    public int slotIndex;                    // �� ��° �������� (0 ~ 29)
    public Image itemIcon;                   // ������ ������ �̹���
    public TextMeshProUGUI amountText;       // ������ ���� �ؽ�Ʈ

    private Inventory.Inven invenData;  // �ش� ������ ������

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
}
