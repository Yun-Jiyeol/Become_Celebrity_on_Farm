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

        OnStatChanged?.Invoke(); // UI �ʱ� ������Ʈ
    }

    //�÷��̾ ������ ���
    [Header("Currency")]
    [SerializeField] private int gold = 0;

    /// ���� ��� ��ȯ
    public int GetGold()
    {
        return gold;
    }

    /// ��� ����
    public void AddGold(int amount)
    {
        gold += Mathf.Max(0, amount); // ���� �Է� ����
    }

    /// ��� ���� . ����� ���� ture��ȯ
    public bool SpendGold(int amount)
    {
        if(gold >= amount)
        {
            gold -= amount;
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
            return false;
        }
    }

    public void SetCharacterInfo(string characterType, string name, string farmName) //ĳ���� ���� ������ ��ȯ�� �޼���
    {
        CharacterType = characterType;
        Name = name;
        FarmName = farmName;

        Debug.Log($"[PlayerStats] ĳ���� �����Ϸ� - {CharacterType}, {Name}, {FarmName}");
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

    // ü��, ���¹̳� ���� �˸��� �̺�Ʈ
    public event Action OnStatChanged;
}
