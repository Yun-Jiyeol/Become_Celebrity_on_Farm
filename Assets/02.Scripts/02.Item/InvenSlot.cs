using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    public int slotnum;
    public Image image;
    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        TestManager.Instance.SlotItem[slotnum-1] = this;
    }

    public void SettingSlotUI()
    {
        if (GameManager.Instance.player.GetComponent<Player>().inventory.PlayerHave.Count <= slotnum - 1) return;

        Inventory.Inven checkItem = GameManager.Instance.player.GetComponent<Player>().inventory.PlayerHave[slotnum - 1];

        if (checkItem == null)
        {
            image.sprite = null;
            textMeshPro.text = string.Empty;
        }
        else
        {
            image.sprite = ItemManager.Instance.itemDataReader.itemsDatas[checkItem.ItemData_num].Item_sprite;
            textMeshPro.text = checkItem.amount.ToString();
        }
    }
}
