using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopExplainPrice : MonoBehaviour
{
    private RectTransform rect;
    public TextMeshProUGUI LeftBtnPrice;
    public TextMeshProUGUI RightBtnPrice;

    public void Setting(int One, int All)
    {
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
