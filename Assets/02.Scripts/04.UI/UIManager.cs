using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("�ټ��� UIManager ����!");
            Destroy(gameObject);
            return;
        }
    }
}
