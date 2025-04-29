using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
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
        if (player == null) return;

        hpFillImage.fillAmount = player.Hp / player.MaxHp;
        Debug.Log($"[UIHpBar] Hp 갱신: {player.Hp} / {player.MaxHp}");
    }
}