using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Season;

/// <summary>
/// 스폰이 될 타일맵에 붙임
/// ex) FarmGround, MineGround
/// </summary>
public class StuffSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private List<GameObject> startStuffs;      // 첫 날 나올 프리팹 리스트
    [SerializeField] private List<GameObject> nextDayStuffs;    // 하루 지난 후 반복해서 나올 프리팹 리스트

    [SerializeField] private Transform onActiveObjs;

    public int prefabCount;

    readonly HashSet<Vector3Int> usedPositions = new();

    bool isSpawned = false;
    bool isFirstDay = true;

    Season season;

    void Start()
    {
        season = TimeManager.Instance.season;
        TimeManager.Instance.OnDayChanged += ExtraSpawn;
        season.OnSeasonChanged += ChangePrefabSeason;
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
        var prefabList = isFirstDay || nextDayStuffs.Count == 0 ? startStuffs : nextDayStuffs;

        for (int i = 0; i < count; i++)
        {
            Vector3 position = SetRandomPosition();
            GameObject prefab = SetStuff(prefabList);
            Instantiate(prefab, position, Quaternion.identity, onActiveObjs);
        }

        isFirstDay = false;
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
    /// 리스트 받아서 랜덤 프리팹 리턴
    /// </summary>
    GameObject SetStuff(List<GameObject> stuffList)
    {
        int num = Random.Range(0, stuffList.Count);
        GameObject stuff = stuffList[num];
        return stuff;
    }

    /// <summary>
    /// 기존에 깔려있던 프리팹 계절 스프라이트 변경
    /// </summary>
    void ChangePrefabSeason(SeasonType newSeason)
    {
        GameManager.Instance.OneSeasonAfter();

        foreach (var sg in onActiveObjs.GetComponentsInChildren<SeedGrow>())
        {
            sg.Grow(0f);
        }
    }
}
