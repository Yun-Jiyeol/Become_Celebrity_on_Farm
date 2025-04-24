using UnityEngine;

/// <summary>
/// 각각의 입구는 이 스크립트를 가짐
/// </summary>
public class MapTransition : MonoBehaviour
{
    [SerializeField] private MapType targetType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        MapManager.Instance.LoadMap(targetType, this.gameObject);
        Debug.Log("Collided");
    }
}