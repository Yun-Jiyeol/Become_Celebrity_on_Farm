using UnityEngine;

public class Food : MonoBehaviour
{
    public float existTime = 10f; // ���̸� �� ������ 10�� �� �����

    void Start()
    {
        Destroy(gameObject, existTime);
    }
}

