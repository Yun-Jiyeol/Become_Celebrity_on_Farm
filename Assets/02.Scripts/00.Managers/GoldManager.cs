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

    public int GetGold() //외부에서 현재 골드 값을 가져올수 있게 해주는 함수
    {
        return player != null ? player.GetGold() : 0;
    }

    public void AddGold(int amount) //PlayerStats의 골드를 증가시킴.
    {
        if (player == null) return;

        player.AddGold(amount);
        OnGoldChanged?.Invoke(player.GetGold());
    }

    public bool SpendGold(int amount) //충분한 골드가 있다면 골드 차감.
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
        }
        return result;
    }
}
