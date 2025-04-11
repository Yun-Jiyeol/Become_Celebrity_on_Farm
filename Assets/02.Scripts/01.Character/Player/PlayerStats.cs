using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Need")]
    public string Name;
    public string FarmName;
    public string CharacterType; //male/female
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

    public void SetCharacterInfo(string characterType, string name, string farmName) //캐릭터 설정 데이터 변환용 메서드
    {
        CharacterType = characterType;
        Name = name;
        FarmName = farmName;

        Debug.Log($"[PlayerStats] 캐릭터 설정완료 - {CharacterType}, {Name}, {FarmName}");
    }
}
