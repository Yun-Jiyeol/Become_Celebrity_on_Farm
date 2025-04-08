using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Need")]
    public float Hp;
    public float MaxHp;
    public float Speed;
    public float Attack;

    [Header("Select")]
    public float Mana;
    public float MaxMana;
    public float Defence;

    public float GetItemRange;
}
