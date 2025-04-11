using UnityEngine;
using UnityEngine.UI;

public class PlayerConditions : UIBase
{
    [Header("Fill Images")]
    public Image hpFillBar;
    public Image energyFillBar;
    public Image stressFillBar;

    [Header("Player Stats")]
    public float maxHp = 100f;
    public float maxEnergy = 100f;
    public float maxStress = 100f;

    private float currentHp;
    private float currentEnergy;
    private float currentStress;

    public override void Show()
    {
        base.Show();

        UpdateUI();
    }
    void Start()
    {
        currentHp = maxHp;
        currentEnergy = maxEnergy;
        currentStress = 0f;

        UpdateUI();
    }

    void Update()
    {
        // 테스트 입력 (나중에 삭제 또는 변경)
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentHp = Mathf.Max(0f, currentHp - 10f);
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentEnergy = Mathf.Max(0f, currentEnergy - 5f);
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentStress = Mathf.Min(maxStress, currentStress + 10f);

        UpdateUI();
    }

    void UpdateUI()
    {
        hpFillBar.fillAmount = currentHp / maxHp;
        energyFillBar.fillAmount = currentEnergy / maxEnergy;
        stressFillBar.fillAmount = currentStress / maxStress;
    }

    public void SetStats(float hp, float energy, float stress)
    {
        currentHp = Mathf.Clamp(hp, 0, maxHp);
        currentEnergy = Mathf.Clamp(energy, 0, maxEnergy);
        currentStress = Mathf.Clamp(stress, 0, maxStress);

        UpdateUI();
    }
}
