using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;
    
    public int CurGold { get; private set; }

    public event Action<int> OnGoldChanged; //골드가 변경될 때 ui등에 알리는 이벤트(매개변수로 현재 골드 값 전달)

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

    public void AddGold(int amount) //골드 amount만큼 증가, 변경 이벤트 발생
    {
        CurGold += amount;
        OnGoldChanged?.Invoke(CurGold);
    }

    public bool SpendGold(int amount) //골드 amount만큼 사용
    {
        if(CurGold >= amount)
        {
            CurGold -= amount;
            OnGoldChanged?.Invoke(CurGold);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족함!");
            return false;
        }
    }

    public void SetGold(int amount) //골드를 특정값으로 설정. 음수가 되지 않도록 0 이상으로 유지
    {
        CurGold = Mathf.Max(0, amount);
        OnGoldChanged?.Invoke(CurGold);
    }
}
