using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GoldUI : UIBase
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        Debug.Log("[GoldUI] OnEnable ȣ���");

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(GoldManager.Instance.GetGold());
        }
        else
        {
            Debug.LogWarning("[GoldUI] OnEnable���� GoldManager.Instance�� null�Դϴ�.");
        }
    }
    private void Start()
    {
        Debug.Log("[GoldUI] Start ȣ���");

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(GoldManager.Instance.GetGold());
        }
        else
        {
            Debug.LogWarning("[GoldUI] Start���� GoldManager.Instance�� null�Դϴ�.");
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
        Debug.Log($"[GoldUI] ��� UI ���ŵ�: {amount}");
        goldText.text = amount.ToString("F0"); // �Ҽ��� ����, ��ǥ ����
    }
}
