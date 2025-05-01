using UnityEngine;
using UnityEngine.UI;

public class UIEnergyBar : MonoBehaviour
{
    [SerializeField] private Image energyFillImage;
    private PlayerStats player;

    public void Init(PlayerStats target)
    {
        player = target;
        player.OnStatChanged -= UpdateUI; // Ȥ�ö� �ߺ� ���� ����
        player.OnStatChanged += UpdateUI;
        UpdateUI(); // ���� ������Ʈ
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnStatChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (player == null || player.MaxMana <= 0) return; // NaN ����
        energyFillImage.fillAmount = player.Mana / player.MaxMana;
    }
}