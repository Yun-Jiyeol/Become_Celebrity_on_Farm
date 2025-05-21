using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopExplainPrice : MonoBehaviour
{
    private RectTransform rect;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI LeftBtnPrice;
    public TextMeshProUGUI RightBtnPrice;
    public TextMeshProUGUI LeftBtnText;
    public TextMeshProUGUI RightBtnText;

    public void Setting(string _ItemName, int One, int All)
    {
        ItemName.text = _ItemName;
        LeftBtnPrice.text = One.ToString();
        RightBtnPrice.text = All.ToString();
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rect.anchoredPosition = Input.mousePosition;
    }
}
