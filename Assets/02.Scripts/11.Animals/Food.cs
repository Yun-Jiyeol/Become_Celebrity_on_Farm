using UnityEngine;

public class Food : MonoBehaviour
{
    public float existTime = 10f; // 먹이를 안 먹으면 10초 뒤 사라짐

    void Start()
    {
        Destroy(gameObject, existTime);
    }
}

