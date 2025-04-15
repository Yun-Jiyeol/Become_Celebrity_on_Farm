using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : UIBase
{
    [SerializeField] private Image hpFillBar;
    private PlayerStats player;

    public void Init(PlayerStats targetPlayer)
    {
        player = targetPlayer;
        UpdateUI();
    }
    private void Update()
    {
        if (player == null || player.MaxHp <= 0) return;
        hpFillBar.fillAmount = player.Hp / player.MaxHp;
    }

    private void UpdateUI()
    {
        if (player == null) return;

        hpFillBar.fillAmount = player.Hp / player.MaxHp;
    }
}
