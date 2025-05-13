using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipObj;
    public TextMeshProUGUI nameText;      //아이템 이름
    public TextMeshProUGUI descText;      //설명
    public TextMeshProUGUI extraInfoText; //추가정보
    public TextMeshProUGUI typeText;          //아이템 종류

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (tooltipObj.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;

            // 마우스 우측 아래 살짝 띄워서 따라다니기
            mousePos += new Vector3(30, -20, 0);

            // Tooltip 위치 고정
            tooltipObj.transform.position = mousePos;
        }
    }

    public void ShowTooltip(ItemDataReader.ItemsData itemData, Vector3 pos)
    {
        tooltipObj.SetActive(true);
        tooltipObj.transform.position = pos;

        nameText.text = itemData.Item_name;
        descText.text = itemData.Item_Explain;
        typeText.text = itemData.Item_Type.ToString();
        extraInfoText.text = GetExtraInfo(itemData);
        
    }

    public void HideTooltip()
    {
        tooltipObj.SetActive(false);
    }

    private string GetExtraInfo(ItemDataReader.ItemsData itemData) //추후에 계속 수정되거나 추가 될 수 있어요
    {
        string result = "";

        switch (itemData.Item_Type)
        {
            case ItemType.Sword:
            case ItemType.Axe:
            case ItemType.Pickaxe:
            case ItemType.Sickle:
                result += $"공격력 : {itemData.Damage}\n";
                break;

            case ItemType.Food:
                result += $"스태미나 회복 : {itemData.Stamina}\n";
                result += $"체력 회복 : {itemData.Hp}\n";
                break;
        }

        if (itemData.Item_Price >= 0)
            result += $"판매가격 : {itemData.Item_Price} 골드\n";

        return result;
    }
}

