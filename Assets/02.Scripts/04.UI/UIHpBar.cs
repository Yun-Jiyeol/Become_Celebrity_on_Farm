using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
    private PlayerStats player;

    public void Init(PlayerStats target)
    {
        player = target;
    }

    private void Update()
    {
        if (player == null) return;

        hpFillImage.fillAmount = player.Hp / player.MaxHp;
    }
}