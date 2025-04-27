using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// 스폰이 될 타일맵에 붙임
/// ex) FarmGround, MineGround
/// </summary>
public class StuffSpawner : MonoBehaviour
{
    // 생각할 것: 중복 스폰 막기

    // 스폰될 타일맵
    [SerializeField] private Tilemap tilemap;
    // 스폰될 프리팹 리스트
    [SerializeField] private List<GameObject> stuffs;

    /// <summary>
    /// 랜덤 위치, 랜덤 프리팹 구해서 맵에 Instantiate
    /// </summary>
    public void SpawnStuff()
    {
        Vector3 position = SetRandomPosition();
        GameObject stuff = SetRandomStuff();
        Instantiate(stuff, position, Quaternion.identity);
    }

    /// <summary>
    /// 1. 랜덤 위치 정하기
    /// </summary>
    Vector3 SetRandomPosition()
    {
        // 해당 프리팹의 전체 크기
        BoundsInt wholeBounds = tilemap.cellBounds;

        for (int i = 0; i < 100; i++)   // test. object pooling?
        {
            int x = Random.Range(wholeBounds.xMin, wholeBounds.xMax);
            int y = Random.Range(wholeBounds.yMin, wholeBounds.yMax);
            Vector3Int tile = new(x, y, 0);

            // 실제로 타일이 있는지 체크
            if (tilemap.HasTile(tile))
            {
                Vector3 pos = tilemap.GetCellCenterWorld(tile);
                return pos;
            }
        }

        return Vector3.zero;
    }

    /// <summary>
    /// 2. 랜덤 프리팹 정하기
    /// </summary>
    /// <returns></returns>
    GameObject SetRandomStuff()
    {
        int num = Random.Range(0, stuffs.Count);
        GameObject stuff = stuffs[num];

        return stuff;
    }
}
