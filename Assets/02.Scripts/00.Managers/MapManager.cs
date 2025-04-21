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
/// 맵 정보를 담을 클래스
/// </summary>
[System.Serializable]
public class MapInfo
{
    // 해당 맵 오브젝트
    public GameObject place;
    // 입구와 스폰포인트 연결
    public Dictionary<GameObject, Transform> portals = new Dictionary<GameObject, Transform>();

    public MapInfo(GameObject place)
    {
        this.place = place;
    }
}


/// <summary>
/// 맵 로직 매니저
/// </summary>
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Map")]
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject farm;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject village;

    [Header("Entrance")]
    [SerializeField] private GameObject fromHomeToFarm;
    [SerializeField] private GameObject fromFarmToHome;
    [SerializeField] private GameObject fromFarmToRoad;
    [SerializeField] private GameObject fromRoadToFarm;
    [SerializeField] private GameObject fromRoadToVillage;
    [SerializeField] private GameObject fromVillageToRoad;

    [Header("SpawnPoint")]
    [SerializeField] private Transform homeCenter;
    [SerializeField] private Transform homeDown;
    [SerializeField] private Transform farmCenter;
    [SerializeField] private Transform farmDown;
    [SerializeField] private Transform roadUp;
    [SerializeField] private Transform roadRight;
    [SerializeField] private Transform villageLeft;

    Dictionary<MapType, MapInfo> maps;
    MapType currentMap = MapType.Home;

    [Header("Fader")]
    [SerializeField] private LoadingFader fader;


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

        home.SetActive(true);
        player.transform.position = homeCenter.position;
    }

    /// <summary>
    /// 시작할 때 맵 세팅
    /// </summary>
    void SetMap()
    {
        // 맵 정보 세팅
        MapInfo homeInfo = new(home);
        homeInfo.portals.Add(fromHomeToFarm, farmCenter);

        MapInfo farmInfo = new(farm);
        farmInfo.portals.Add(fromFarmToHome, homeDown);
        farmInfo.portals.Add(fromFarmToRoad, roadUp);

        MapInfo roadInfo = new(road);
        roadInfo.portals.Add(fromRoadToFarm, farmDown);
        roadInfo.portals.Add(fromRoadToVillage, villageLeft);

        MapInfo villageInfo = new(village);
        villageInfo.portals.Add(fromVillageToRoad, roadRight);

        // 매핑위한 딕셔너리 추가
        maps = new Dictionary<MapType, MapInfo>()
        {
            { MapType.Home, homeInfo },
            { MapType.Farm, farmInfo },
            { MapType.Road, roadInfo },
            { MapType.Village, villageInfo },
        };
    }

    /// <summary>
    /// 플레이어가 이동할 때 목적지 맵 활성화
    /// </summary>
    public void LoadMap(MapType targetType, GameObject entrance)
    {
        StartCoroutine(fader.Fade(() =>
        {
            // 1. 현재 맵 비활성화
            UnloadMap(currentMap);

            // 2. 타겟 맵 활성화
            if (maps.TryGetValue(targetType, out MapInfo targetMapInfo))
            {
                targetMapInfo.place.SetActive(true);
                SetPlayerPosition(entrance);
                currentMap = targetType;
            }
        }));
    }

    /// <summary>
    /// 플레이어가 이동할 때 원래 있던 맵 비활성화
    /// </summary>
    void UnloadMap(MapType currentMap)
    {
        if (maps.TryGetValue(currentMap, out MapInfo targetMap))
        {
            targetMap.place.SetActive(false);
        }
    }

    /// <summary>
    /// Start 때 한 번 호출. 전체 맵 비활성화
    /// </summary>
    void UnloadAllMap()
    {
        foreach (KeyValuePair<MapType, MapInfo> map in maps)
        {
            map.Value.place.SetActive(false);
        }
    }

    /// <summary>
    /// entrance에 따른 플레이어 스폰위치 설정
    /// </summary>
    /// <param name="entrance"></param>
    void SetPlayerPosition(GameObject entrance)
    {
        if (maps.TryGetValue(currentMap, out MapInfo currentMapInfo))
        {
            if (currentMapInfo.portals.TryGetValue(entrance, out Transform spawnPoint))
            {
                player.transform.position = spawnPoint.position;
            }
        }
    }
}
