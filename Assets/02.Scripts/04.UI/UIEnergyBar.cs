using UnityEngine;
using UnityEngine.UI;

public class UIEnergyBar : MonoBehaviour
{
    [SerializeField] private Image energyFillImage;
    private PlayerStats player;

    public void Init(PlayerStats target)
    {
        player = target;
    }

    private void Update()
    {
        if (player == null) return;

        energyFillImage.fillAmount = player.Mana / player.MaxMana;
    }
}