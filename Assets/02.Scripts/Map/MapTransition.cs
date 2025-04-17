using UnityEngine;

/// <summary>
/// 각각의 입구는 이 스크립트를 가짐
/// </summary>
public class MapTransition : MonoBehaviour
{
    MapType targetPlace;
    string spawnPointName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        DivisionObjectName();
        MapManager.Instance.LoadMap(targetPlace, spawnPointName);
        Debug.Log("Collided");
    }

    /// <summary>
    /// 오브젝트 이름으로 출발지, 도착지 구분
    /// 오브젝트 이름 형식 = From_출발지_To_도착지
    /// </summary>
    void DivisionObjectName()
    {
        string[] nameParts = gameObject.name.Split('_');
        targetPlace = (MapType)System.Enum.Parse(typeof(MapType), nameParts[3]);
        spawnPointName = "Spawn" + nameParts[3];
    }
}
