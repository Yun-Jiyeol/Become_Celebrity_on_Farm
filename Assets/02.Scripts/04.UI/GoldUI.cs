using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GoldUI : UIBase
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(GoldManager.Instance.GetGold());
        }
    }

    private void OnDisable()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int value)
    {
        goldText.text = value.ToString("N0"); // √µ ¥‹¿ß Ω∞«•
    }
}
