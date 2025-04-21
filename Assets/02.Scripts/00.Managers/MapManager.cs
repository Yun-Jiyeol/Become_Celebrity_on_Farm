using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    Home,
    Farm,
    Road,
    Village
}

/// <summary>
/// 맵 정보를 담은 클래스
/// </summary>
[System.Serializable]
public class MapInfo
{
    // Field
    private GameObject _place;
    private Dictionary<string, Transform> _spawnPoints;

    // Property
    public GameObject Place => _place;
    public Dictionary<string, Transform> SpawnPoints => _spawnPoints;

    // Constructor
    public MapInfo(GameObject place)
    {
        _place = place;
        _spawnPoints = new Dictionary<string, Transform>();
    }

    // Method
    public void AddSpawnPoint(string name, Transform spawnPoint)
    {
        _spawnPoints.Add(name, spawnPoint);
    }
}


/// <summary>
/// 맵 로직 매니저
/// </summary>
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    // 쉬운 매핑 위한 Dictionary
    private Dictionary<MapType, MapInfo> places;

    [Header("Map Objects")]
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject farm;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject village;

    [Header("SpawnPoints")]
    [SerializeField] private Transform SpawnHome;
    [SerializeField] private Transform SpawnFarmUpward;
    [SerializeField] private Transform SpawnFarmDownward;
    [SerializeField] private Transform SpawnRoadUpward;
    [SerializeField] private Transform SpawnRoadDownward;
    [SerializeField] private Transform SpawnVillage;

    // 현재 MapType. 디폴트 = 집
    MapType currentMap = MapType.Home;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetMap();
        UnloadAllMap();
        LoadMap(MapType.Home, "SpawnHome");
    }


    /// <summary>
    /// 맵 정보 세팅
    /// </summary>
    void SetMap()
    {
        // MapInfo 생성
        MapInfo homeInfo = new(home);
        homeInfo.AddSpawnPoint("SpawnFarm", SpawnHome);

        MapInfo farmInfo = new(farm);
        farmInfo.AddSpawnPoint("SpawnHome", SpawnFarmUpward);
        farmInfo.AddSpawnPoint("SpawnRoad", SpawnFarmDownward);

        MapInfo roadInfo = new(road);
        roadInfo.AddSpawnPoint("SpawnFarm", SpawnRoadUpward);
        roadInfo.AddSpawnPoint("SpawnVillage", SpawnRoadDownward);

        MapInfo villageInfo = new(village);
        villageInfo.AddSpawnPoint("SpawnRoad", SpawnVillage);

       // Dictionary에 MapInfo 추가
        places = new Dictionary<MapType, MapInfo>()
        {
            { MapType.Home, homeInfo },
            { MapType.Farm, farmInfo },
            { MapType.Road, roadInfo },
            { MapType.Village, villageInfo }
        };
    }

    /// <summary>
    /// 맵 활성화
    /// </summary>
    /// <param name="targetType"> 플레이어가 다음에 갈 맵 </param>
    /// <param name="spawnPointName"> 다음에 갈 맵에서 플레이어가 스폰될 위치 </param>
    public void LoadMap(MapType targetType, string spawnPointName)
    {
        // 1. 현재 맵 비활성화
        UnloadMap(currentMap);

        // 2. 타겟 맵 활성화
        if (places.TryGetValue(targetType, out MapInfo targetMap))
        {
            targetMap.Place.SetActive(true);
            SetPlayerPosition(targetType, spawnPointName);
            currentMap = targetType;

            Debug.Log($"현재 장소: {targetType}");
        }
    }

    /// <summary>
    /// 맵 비활성화
    /// </summary>
    /// <param name="currentMapType"> 플레이어가 현재 있는 맵 </param>
    void UnloadMap(MapType currentMapType)
    {
        if (places.TryGetValue(currentMapType, out MapInfo targetMap))
            targetMap.Place.SetActive(false);
    }

    /// <summary>
    /// Start 때 한 번 호출. 전체 맵 비활성화
    /// </summary>
    void UnloadAllMap()
    {
        foreach (KeyValuePair<MapType, MapInfo> place in places)
        {
            place.Value.Place.SetActive(false);
        }
    }

    /// <summary>
    /// 플레이어 스폰 위치 세팅
    /// </summary>
    /// <param name="spawnPointName"></param>
    void SetPlayerPosition(MapType targetType, string spawnPointName)
    {
        GameObject player = GameObject.FindWithTag("Player");       // * 최적화 고려

        if (places[targetType].SpawnPoints.TryGetValue(spawnPointName, out Transform transform))
        {
            player.transform.position = transform.position;
        }
    }
}
