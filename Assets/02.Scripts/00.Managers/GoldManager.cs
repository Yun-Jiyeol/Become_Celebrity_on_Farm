using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;
    
    public int CurGold { get; private set; }

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

    public void AddGold(int amount) //��� amount��ŭ ����, ���� �̺�Ʈ �߻�
    {
        CurGold += amount;
        OnGoldChanged?.Invoke(CurGold);
    }

    public bool SpendGold(int amount) //��� amount��ŭ ���
    {
        if(CurGold >= amount)
        {
            CurGold -= amount;
            OnGoldChanged?.Invoke(CurGold);
            return true;
        }
        else
        {
            Debug.Log("��尡 ������!");
            return false;
        }
    }

    public void SetGold(int amount) //��带 Ư�������� ����. ������ ���� �ʵ��� 0 �̻����� ����
    {
        CurGold = Mathf.Max(0, amount);
        OnGoldChanged?.Invoke(CurGold);
    }
}
