using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Need")]
    public string Name;
    public float Hp;
    public float MaxHp;
    public float Speed;
    public float Attack;

    public float GetItemRange;
    public float ActiveRange;

    [Header("Select")]
    public float Mana;
    public float MaxMana;
    public float Defence;
}
