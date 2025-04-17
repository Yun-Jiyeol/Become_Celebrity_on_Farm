using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GoldUI : UIBase
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        if(GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(GoldManager.Instance.CurGold); // 시작 시 초기화
        }
    }

    private void OnDestroy()
    {
        if(GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int gold)
    {
        goldText.text = gold.ToString("0000000");
    }

    public override void Show()
    {
        base.Show();
        UpdateUI(GoldManager.Instance.CurGold);
    }
}
