using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    private PlayerStats player;

    public event Action<int> OnGoldChanged; //��尡 ����� �� ui� �˸��� �̺�Ʈ(�Ű������� ���� ��� �� ����)

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
            Debug.LogError("[GoldManager] PlayerStats�� ã�� ����.");
            return;
        }

        OnGoldChanged?.Invoke(player.GetGold()); //���۽� ui�� �ʱ� ��尪 ����
    }

    public int GetGold()
    {
        return player != null ? player.GetGold() : 0;
    }

    public void AddGold(int amount) //��� amount��ŭ ����, ���� �̺�Ʈ �߻�
    {
        if (player == null) return;

        player.AddGold(amount);
        OnGoldChanged?.Invoke(player.GetGold());
    }

    public bool SpendGold(int amount) //��� amount��ŭ ���
    {
        if (player == null) return false;

        bool result = player.SpendGold(amount);
        if (result)
        {
            OnGoldChanged?.Invoke(player.GetGold());
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
            return result;
        }
    }
}
