using UnityEngine;

/// <summary>
/// 카메라 범위 콜라이더 변경용
/// </summary>
public class CamConfinerChange : MonoBehaviour
{
    Cinemachine.CinemachineConfiner2D confiner;

    void Awake()
    {
        confiner = GetComponent<Cinemachine.CinemachineConfiner2D>();
    }

    public void ChangeCameraBorder(Collider2D border)
    {
        if (border == null) return;

        confiner.m_BoundingShape2D = border;
        confiner.InvalidateCache();
    }
}
