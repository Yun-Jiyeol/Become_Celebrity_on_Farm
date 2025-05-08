using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;

    [Header("UI")]
    public Image expFillBar;
    public TextMeshProUGUI levelText;

    [Header("����ġ ����")]
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
        UpdateUI(); // ������ �� ���� EXP ������� �� ����
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            level++;
            Debug.Log($"[ExpManager] ������! ���� ����: {level}");
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