using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 각각의 입구는 이 스크립트를 가짐
/// </summary>
public class MapTransition : MonoBehaviour
{
    [SerializeField] private MapType targetType;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Mine은 UI에서 선택되기 때문에 다르게 처리
        if (targetType == MapType.Mine)
        {
            MapManager.Instance.mineSelectUI.SetMineEntrance(this);
            MapManager.Instance.mineSelectUI.Show();
         
            if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
                input.enabled = false;
        }
        else
        {
            MapManager.Instance.LoadMap(targetType, this.gameObject);
        }

        //Debug.Log("Collided");
    }

    /// <summary>
    /// MineEntrance에서 사용
    /// </summary>
    public void SelectMine(MapType selectedType)
    {
        MapManager.Instance.LoadMap(selectedType, this.gameObject);
    }
}
