using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    private PlayerStats player;

    public event Action<int> OnGoldChanged; //골드가 변경될 때 ui등에 알리는 이벤트(매개변수로 현재 골드 값 전달)

    // 하루 정산용. 사용/소비한 골드 저장
    [HideInInspector] public int addAmount = 0;
    [HideInInspector] public int spendAmount = 0;

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
    }

    private void Start()
    {
        player = GameManager.Instance.player.GetComponent<Player>().stat;
        if(player == null)
        {
            Debug.LogError("[GoldManager] PlayerStats를 찾지 못함.");
            return;
        }

        OnGoldChanged?.Invoke(player.GetGold()); //시작시 ui에 초기 골드값 전달
        TimeManager.Instance.OnDayChanged += ResetStoredGold;
    }

    public int GetGold() //외부에서 현재 골드 값을 가져올수 있게 해주는 함수
    {
        return player != null ? player.GetGold() : 0;
    }

    public void AddGold(int amount) //PlayerStats의 골드를 증가시킴.
    {
        if (player == null) return;

        player.AddGold(amount);
        addAmount += amount;
        OnGoldChanged?.Invoke(player.GetGold());
    }

    public bool SpendGold(int amount) //충분한 골드가 있다면 골드 차감.
    {
        if (player == null) return false;

        bool result = player.SpendGold(amount);
        if (result)
        {
            spendAmount += amount;
            OnGoldChanged?.Invoke(player.GetGold());
        }
        else
        {
            Debug.Log("골드가 부족합니다.");           
        }
        return result;
    }


    void ResetStoredGold()      // 하루 지나면 리셋
    {
        addAmount = 0;
        spendAmount = 0;
    }
}
