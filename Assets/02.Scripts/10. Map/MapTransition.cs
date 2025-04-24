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

        if (targetType == MapType.Mine)
        {
            MapManager.Instance.mineSelectUI.SetMineEntrance(this);
            MapManager.Instance.mineSelectUI.Show();
         
            if (GameManager.Instance.player.TryGetComponent(out PlayerController controller))
                controller.enabled = false;
        }
        else
        {
            MapManager.Instance.LoadMap(targetType, this.gameObject);
        }

        Debug.Log("Collided");
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
