using UnityEngine;
using UnityEngine.UI;

public class UIEnergyBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private PlayerStats player;

    public void Init(PlayerStats target)
    {
        player = target;
        player.OnStatChanged += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        fillImage.fillAmount = player.Mana / player.MaxMana;
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnStatChanged -= UpdateUI;
    }
}