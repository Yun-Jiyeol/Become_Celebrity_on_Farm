using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    private PlayerStats player;

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

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        if(player == null)
        {
            Debug.LogError("[GoldManager] PlayerStats를 찾지 못함.");
            return;
        }

        OnGoldChanged?.Invoke(player.GetGold()); //시작시 ui에 초기 골드값 전달
    }

    public int GetGold()
    {
        return player != null ? player.GetGold() : 0;
    }

    public void AddGold(int amount) //골드 amount만큼 증가, 변경 이벤트 발생
    {
        if (player == null) return;

        player.AddGold(amount);
        OnGoldChanged?.Invoke(player.GetGold());
    }

    public bool SpendGold(int amount) //골드 amount만큼 사용
    {
        if (player == null) return false;

        bool result = player.SpendGold(amount);
        if (result)
        {
            OnGoldChanged?.Invoke(player.GetGold());
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
            return result;
        }
    }
}
