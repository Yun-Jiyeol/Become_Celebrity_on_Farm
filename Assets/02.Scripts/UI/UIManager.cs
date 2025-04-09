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
            Debug.LogWarning("다수의 UIManager 존재!");
            Destroy(gameObject);
            return;
        }
    }
}
