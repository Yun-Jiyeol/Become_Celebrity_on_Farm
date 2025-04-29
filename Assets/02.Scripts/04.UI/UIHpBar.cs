using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
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
        if (player == null) return;

        hpFillImage.fillAmount = player.Hp / player.MaxHp;
        Debug.Log($"[UIHpBar] Hp ����: {player.Hp} / {player.MaxHp}");
    }
}