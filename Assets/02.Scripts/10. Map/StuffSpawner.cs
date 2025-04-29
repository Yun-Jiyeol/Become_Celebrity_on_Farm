using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// 스폰이 될 타일맵에 붙임
/// ex) FarmGround, MineGround
/// </summary>
public class StuffSpawner : ObjectPolling
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<GameObject> stuffs;
    [SerializeField] private Transform onActiveObj;
    public int prefabCount = 100;
    private int curDay;

    readonly HashSet<Vector3Int> usedPositions = new();

    bool isSpawned = false;


    void Awake()
    {
        InitializeStuffPool(prefabCount);
    }

    void Start()
    {
        if (TimeManager.Instance != null)
            curDay = TimeManager.Instance.currentDay;
    }

    void Update()
    {
        if (curDay != TimeManager.Instance.currentDay)
        {
            Debug.Log($"[StuffSpawner] 날짜 달라짐. {curDay}, {TimeManager.Instance.currentDay}");
            curDay = TimeManager.Instance.currentDay;
            SpawnStuff(10);
        }
    }

    void OnEnable()
    {
        Debug.Log($"[StuffSpawner] 날짜 초기화. {curDay}, {TimeManager.Instance.currentDay}");

        if (!isSpawned)
        {
            SpawnStuff(prefabCount);
            isSpawned = true;
        }
    }

    /// <summary>
    /// 랜덤 프리팹으로 오브젝트 풀 초기화
    /// </summary>
    /// <param name="count">오브젝트 개수</param>
    void InitializeStuffPool(int count)
    {
        for (int i = 0; i < count ; i++)
        {
            // 1. 랜덤으로 프리팹 추출
            int num = Random.Range(0, stuffs.Count);
            GameObject stuff = stuffs[num];

            // 2. 프리팹 생성
            GameObject obj = Instantiate(stuff, onActiveObj);
            obj.SetActive(false);
            Things.Add(obj);
        }
    }

    /// <summary>
    /// 랜덤 위치에 랜덤 프리팹 스폰
    /// </summary>
    void SpawnStuff(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = SetRandomPosition();
            GameObject obj = SetStuff();
            obj.transform.position = position;
            obj.SetActive(true);
        }
    }

    /// <summary>
    /// 랜덤 위치 정하기
    /// </summary>
    Vector3 SetRandomPosition()
    {
        // 해당 프리팹의 전체 크기
        BoundsInt wholeBounds = tilemap.cellBounds;

        int tryCount = 0;
        int maxTry = 200;

        // 위치 찾을 때까지 200번 반복
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
    /// 프리팹 리턴
    /// </summary>
    GameObject SetStuff()
    {
        GameObject stuff = SpawnOrFindThings();
        return stuff;
    }
}
