using TMPro;
using UnityEngine;

public class GoldUIController : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI currentMText;
    public TextMeshProUGUI totalMText;

    private int totalM = 0;

    private void Start()
    {
        // 골드 변경 이벤트 연결
        GoldManager.Instance.OnGoldChanged += UpdateCurrentGold;

        // 하루가 바뀌면 TotalM 누적
        TimeManager.Instance.OnDayChanged += AddToTotalGold;

        // 초기 세팅
        UpdateCurrentGold(GoldManager.Instance.GetGold());
        AddToTotalGold(); // 첫날 정산값 반영
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

