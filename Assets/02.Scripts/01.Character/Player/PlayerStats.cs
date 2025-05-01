using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    [Header("Need")]
    public string Name;
    public string FarmName;
    public string CharacterType; // male/female
    public float Hp;
    public float MaxHp;
    public float Speed;
    public float Attack;

    public float GetItemRange;
    public float ActiveRange;
    public int InventorySize;

    [Header("Select")]
    public float Mana;
    public float MaxMana;
    public float Defence;

    void Start()
    {
        MaxHp = 100;
        Hp = MaxHp;

        MaxMana = 100;
        Mana = MaxMana;

        Speed = 7;
        Attack = 10;
        Defence = 5;

        InventorySize = 12;
        GetItemRange = 2;
        ActiveRange = 1.5f;

        OnStatChanged?.Invoke(); // UI 초기 업데이트
    }

    //플레이어가 보유한 골드
    [Header("Currency")]
    [SerializeField] private int gold = 0;

    /// 현재 골드 반환
    public int GetGold()
    {
        return gold;
    }

    /// 골드 증가
    public void AddGold(int amount)
    {
        gold += Mathf.Max(0, amount); // 음수 입력 방지
    }

    /// 골드 감소 . 충분할 때만 ture반환
    public bool SpendGold(int amount)
    {
        if(gold >= amount)
        {
            gold -= amount;
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
            return false;
        }
    }

    public void SetCharacterInfo(string characterType, string name, string farmName) //캐릭터 설정 데이터 변환용 메서드
    {
        CharacterType = characterType;
        Name = name;
        FarmName = farmName;

        Debug.Log($"[PlayerStats] 캐릭터 설정완료 - {CharacterType}, {Name}, {FarmName}");
    }

    public void ChangeHp(float amount)
    {
        Hp = Mathf.Clamp(Hp + amount, 0, MaxHp);
        OnStatChanged?.Invoke();

    }

    public void ChangeMana(float amount)
    {
        Mana = Mathf.Clamp(Mana + amount, 0, MaxMana);
        OnStatChanged?.Invoke();
    }

    // 체력, 스태미나 변경 알림용 이벤트
    public event Action OnStatChanged;
}
