using TMPro;
using UnityEngine;

public class GoldUIController : MonoBehaviour
{
    [Header("UI ����")]
    public TextMeshProUGUI currentMText;
    public TextMeshProUGUI totalMText;

    private int totalM = 0;

    private void Start()
    {
        // ��� ���� �̺�Ʈ ����
        GoldManager.Instance.OnGoldChanged += UpdateCurrentGold;

        // �Ϸ簡 �ٲ�� TotalM ����
        TimeManager.Instance.OnDayChanged += AddToTotalGold;

        // �ʱ� ����
        UpdateCurrentGold(GoldManager.Instance.GetGold());
        AddToTotalGold(); // ù�� ���갪 �ݿ�
    }

    private void UpdateCurrentGold(int currentGold)
    {
        currentMText.text = currentGold.ToString();
    }

    private void AddToTotalGold()
    {
        var goldManager = GoldManager.Instance;
        int dailyTotal = goldManager.addAmount + goldManager.spendAmount;
        totalM += dailyTotal;

        totalMText.text = totalM.ToString();
    }

    private void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= UpdateCurrentGold;

        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayChanged -= AddToTotalGold;
    }
}

