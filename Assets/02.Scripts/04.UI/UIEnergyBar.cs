using UnityEngine;
using UnityEngine.UI;

public class UIEnergyBar : UIBase
{
    [SerializeField] private Image energyFillBar;
    private PlayerStats player;

    public void Init(PlayerStats targetPlayer)
    {
        player = targetPlayer;
        UpdateUI();
    }

    private void Update()
    {
        if (player == null || player.MaxMana <= 0) return;
        energyFillBar.fillAmount = player.Mana / player.MaxMana;
    }

    private void UpdateUI()
    {
        if (player == null) return;

        energyFillBar.fillAmount = player.Hp / player.MaxHp;
    }
}