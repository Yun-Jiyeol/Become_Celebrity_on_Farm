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
    void LateUpdate()
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            if (float.IsNaN(rt.rect.width) || float.IsNaN(rt.rect.height))
                Debug.LogError($"{name}�� RectTransform�� NaN�� ��� ����!");

            if (rt.rect.width <= 0 || rt.rect.height <= 0)
                Debug.LogError($"{name}�� ũ�Ⱑ �ʹ� �۰ų� 0�Դϴ�!");
        }
    }
}
