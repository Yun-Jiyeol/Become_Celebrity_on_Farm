using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
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
        fillImage.fillAmount = player.Hp / player.MaxHp;
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnStatChanged -= UpdateUI;
    }
}