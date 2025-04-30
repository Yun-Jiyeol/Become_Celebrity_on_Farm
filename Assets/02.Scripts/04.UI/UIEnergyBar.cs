using UnityEngine;
using UnityEngine.UI;

public class UIEnergyBar : MonoBehaviour
{
    [SerializeField] private Image energyFillImage;
    private PlayerStats player;

    public void Init(PlayerStats target)
    {
        player = target;
        player.OnStatChanged -= UpdateUI; // 혹시라도 중복 구독 막기
        player.OnStatChanged += UpdateUI;
        UpdateUI(); // 강제 업데이트
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnStatChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (player == null || player.MaxMana <= 0) return; //NaN 방지
        Debug.Log($"[UIEnergyBar] Energy 갱신: {player.Mana} / {player.MaxMana}");
        energyFillImage.fillAmount = player.Mana / player.MaxMana;
    }
}