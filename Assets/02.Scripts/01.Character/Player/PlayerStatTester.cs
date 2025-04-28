using UnityEngine;
using UnityEngine.UI;

public class PlayerStatTester : MonoBehaviour
{
    [SerializeField] private Button hpDecreaseButton;
    [SerializeField] private Button hpIncreaseButton;
    [SerializeField] private Button manaDecreaseButton;
    [SerializeField] private Button manaIncreaseButton;

    private PlayerStats player;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        if (player == null)
        {
            Debug.LogError("[PlayerStatTester] PlayerStats 찾을 수 없음!");
            return;
        }

        hpDecreaseButton.onClick.AddListener(() => ChangeHp(-10f));
        hpIncreaseButton.onClick.AddListener(() => ChangeHp(+10f));
        manaDecreaseButton.onClick.AddListener(() => ChangeMana(-10f));
        manaIncreaseButton.onClick.AddListener(() => ChangeMana(+10f));
    }

    private void ChangeHp(float amount)
    {
        player.Hp = Mathf.Clamp(player.Hp + amount, 0, player.MaxHp);
        Debug.Log($"[TEST] 현재 체력: {player.Hp} / {player.MaxHp}");
    }

    private void ChangeMana(float amount)
    {
        player.Mana = Mathf.Clamp(player.Mana + amount, 0, player.MaxMana);
        Debug.Log($"[TEST] 현재 마나: {player.Mana} / {player.MaxMana}");
    }
}