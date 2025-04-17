using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;
    
    public int CurGold { get; private set; }

    public event Action<int> onGoldChanged;

    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddGold(int amount)
    {
        CurGold += amount;
        onGoldChanged?.Invoke(CurGold);
    }

    public bool SpendGold(int amount)
    {
        if(CurGold >= amount)
        {
            CurGold -= amount;
            onGoldChanged?.Invoke(CurGold);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족함!");
            return false;
        }
    }

    public void SetGold(int amount)
    {
        CurGold = Mathf.Max(0, amount);
        onGoldChanged?.Invoke(CurGold);
    }
}
