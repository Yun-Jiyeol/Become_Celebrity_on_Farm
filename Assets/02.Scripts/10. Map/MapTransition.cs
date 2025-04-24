using UnityEngine;

/// <summary>
/// 각각의 입구는 이 스크립트를 가짐
/// </summary>
public class MapTransition : MonoBehaviour
{
    [SerializeField] private MapType targetType;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 조건1
        if (!collision.CompareTag("Player")) return;

        // 조건2
        if (targetType == MapType.Mine)
        {
            MapManager.Instance.mineSelectUI.SetActive(true);
            
            
        }
        else
        {
            MapManager.Instance.LoadMap(targetType, this.gameObject);
            Debug.Log("Collided");
        }
    }

    /// <summary>
    /// MineEntrance에서 사용
    /// </summary>
    /// <param name="selectedType"></param>
    public void LoadSelectedMine(MapType selectedType)
    {
        MapManager.Instance.LoadMap(selectedType, this.gameObject);
    }
}