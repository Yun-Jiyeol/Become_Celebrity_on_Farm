using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipObj;
    public TextMeshProUGUI nameText;      //������ �̸�
    public TextMeshProUGUI descText;      //����
    public TextMeshProUGUI extraInfoText; //�߰�����
    public TextMeshProUGUI typeText;          //������ ����

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (tooltipObj.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;

            // ���콺 ���� �Ʒ� ��¦ ����� ����ٴϱ�
            mousePos += new Vector3(30, -20, 0);

            // Tooltip ��ġ ����
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

    private string GetExtraInfo(ItemDataReader.ItemsData itemData) //���Ŀ� ��� �����ǰų� �߰� �� �� �־��
    {
        string result = "";

        switch (itemData.Item_Type)
        {
            case ItemType.Sword:
            case ItemType.Axe:
            case ItemType.Pickaxe:
            case ItemType.Sickle:
                result += $"���ݷ� : {itemData.Damage}\n";
                break;

            case ItemType.Food:
                result += $"���¹̳� ȸ�� : {itemData.Stamina}\n";
                result += $"ü�� ȸ�� : {itemData.Hp}\n";
                break;
        }

        if (itemData.Item_Price >= 0)
            result += $"�ǸŰ��� : {itemData.Item_Price} ���\n";

        return result;
    }
}

