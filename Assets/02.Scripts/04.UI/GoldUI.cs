using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GoldUI : UIBase
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        Debug.Log("[GoldUI] OnEnable 호출됨");

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(GoldManager.Instance.GetGold());
        }
        else
        {
            Debug.LogWarning("[GoldUI] OnEnable에서 GoldManager.Instance가 null입니다.");
        }
    }
    private void Start()
    {
        Debug.Log("[GoldUI] Start 호출됨");

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(GoldManager.Instance.GetGold());
        }
        else
        {
            Debug.LogWarning("[GoldUI] Start에서 GoldManager.Instance가 null입니다.");
        }
    }

    private void OnDisable()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int amount)
    {
        Debug.Log($"[GoldUI] 골드 UI 갱신됨: {amount}");
        goldText.text = amount.ToString("F0"); // 소수점 없이, 쉼표 없이
    }
}
