using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;

    [Header("UI")]
    public Image expFillBar;
    public TextMeshProUGUI levelText;

    [Header("경험치 설정")]
    public int level = 1;
    public int currentExp = 0;
    public int maxExp = 100;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI(); // 시작할 때 현재 EXP 기반으로 바 갱신
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            level++;
            Debug.Log($"[ExpManager] 레벨업! 현재 레벨: {level}");
        }

        UpdateUI();
    }
    private void UpdateUI()
    {
        if (expFillBar != null)
            expFillBar.fillAmount = (float)currentExp / maxExp;

        if (levelText != null)
            levelText.text = $"Lv.{level}";
    }
}