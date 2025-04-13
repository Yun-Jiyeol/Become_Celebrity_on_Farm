using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    public int slotnum;
    private Image image;
    private TextMeshProUGUI textMeshPro;


    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        TestManager.Instance.SlotItem[slotnum-1] = this;
    }

    public void SettingSlotUI(int ItemDataNum = 0, int amount = 0)
    {
        if(amount == 0)
        {
            image.gameObject.SetActive(false);
            textMeshPro.text = string.Empty;
        }
        else
        {
            image.gameObject.SetActive(true);
            image.sprite = ItemManager.Instance.itemDataReader.itemsDatas[ItemDataNum].Item_sprite;
            textMeshPro.text = $"{amount}";
        }
    }
}
