using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// 스폰이 될 타일맵에 붙임
/// ex) FarmGround, MineGround
/// </summary>
public class StuffSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<GameObject> stuffs;
    [SerializeField] private Transform onActiveObjs;

    public int prefabCount;

    readonly HashSet<Vector3Int> usedPositions = new();

    bool isSpawned = false;

    void Start()
    {
        TimeManager.Instance.OnDayChanged += ExtraSpawn;
    }

    /// <summary>
    /// 하루 지나면 추가 스폰
    /// </summary>
    void ExtraSpawn()
    {
        SpawnStuff(prefabCount / 20);
    }

    /// <summary>
    /// 처음 켜졌을 때 프리팹 생성
    /// </summary>
    void OnEnable()
    {
        if (!isSpawned)
        {
            SpawnStuff(prefabCount);
            isSpawned = true;
        }
    }

    /// <summary>
    /// 랜덤 위치에 랜덤 프리팹 생성하기
    /// </summary>
    void SpawnStuff(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = SetRandomPosition();
            GameObject obj = SetStuff();
            obj.transform.position = position;
            Instantiate(obj, onActiveObjs);
        }
    }

    /// <summary>
    /// 랜덤 위치 리턴
    /// </summary>
    Vector3 SetRandomPosition()
    {
        // 해당 프리팹의 전체 크기
        BoundsInt wholeBounds = tilemap.cellBounds;

        int tryCount = 0;
        int maxTry = 1000;

        // 위치 찾을 때까지 1000번 반복
        while (tryCount < maxTry)
        {
            int x = Random.Range(wholeBounds.xMin, wholeBounds.xMax);
            int y = Random.Range(wholeBounds.yMin, wholeBounds.yMax);
            Vector3Int tilePos = new(x, y, 0);

            // 실제로 타일이 있는지 체크, 중복 위치 스폰 방지
            if (tilemap.HasTile(tilePos) && !usedPositions.Contains(tilePos))
            {
                usedPositions.Add(tilePos);
                return tilemap.GetCellCenterWorld(tilePos);
            }

            tryCount++;
        }

        Debug.Log("[StuffSpawner] 스폰 위치 찾기 실패");
        return Vector3.zero;
    }

    /// <summary>
    /// 랜덤 프리팹 리턴
    /// </summary>
    GameObject SetStuff()
    {
        int num = Random.Range(0, stuffs.Count);
        GameObject stuff = stuffs[num];
        return stuff;
    }
}
