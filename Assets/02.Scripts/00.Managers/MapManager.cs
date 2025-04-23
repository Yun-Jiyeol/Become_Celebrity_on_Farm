using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MapType
{
    Home,
    Farm,
    Road,
    Village,
    MineEntrance,
    Mine,
    Beach,
}

/// <summary>
/// 맵 정보를 담을 클래스
/// </summary>
[System.Serializable]
public class MapInfo
{
    // 맵 오브젝트
    public GameObject place;
    // 딕셔너리로 입구와 스폰포인트 관리
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


    // 추후 리스트로 변경하기
    [Header("Map")]
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject farm;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject village;
    [SerializeField] private GameObject mineEntrance;
    [SerializeField] private GameObject mine;
    [SerializeField] private GameObject beach;

    [Header("Entrance")]
    [SerializeField] private GameObject fromHomeToFarm;
    [SerializeField] private GameObject fromFarmToHome;
    [SerializeField] private GameObject fromFarmToRoad;
    [SerializeField] private GameObject fromFarmToMineEntrance;
    [SerializeField] private GameObject fromRoadToFarm;
    [SerializeField] private GameObject fromRoadToVillage;
    [SerializeField] private GameObject fromVillageToRoad;
    [SerializeField] private GameObject fromMEToFarm;
    [SerializeField] private GameObject fromMEToMine;
    [SerializeField] private GameObject fromMineToME;     // temp
    [SerializeField] private GameObject fromRoadToBeach;
    [SerializeField] private GameObject fromBeachToRoad;


    [Header("SpawnPoint")]
    [SerializeField] private Transform homeCenter;
    [SerializeField] private Transform homeDown;
    [SerializeField] private Transform farmCenter;
    [SerializeField] private Transform farmDown;
    [SerializeField] private Transform farmRight;
    [SerializeField] private Transform roadUp;
    [SerializeField] private Transform roadRight;
    [SerializeField] private Transform roadDown;
    [SerializeField] private Transform villageLeft;
    [SerializeField] private Transform mEDown;
    [SerializeField] private Transform mEUp;
    [SerializeField] private Transform mineCenter;
    [SerializeField] private Transform beachUp;


    [Header("Fader")]
    [SerializeField] private LoadingFader fader;

    [Header("Camera")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

    Dictionary<MapType, MapInfo> maps;
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

        // 디폴트 = 집에서 스폰
        home.SetActive(true);
        player.transform.position = homeCenter.position;
    }

    /// <summary>
    /// 맵 세팅 시작. 장소 추가될 때마다 맵 생성 필요
    /// </summary>
    void SetMap()
    {
        // 맵 정보 세팅
        MapInfo homeInfo = new(home);
        homeInfo.portals.Add(fromHomeToFarm, farmCenter);

        MapInfo farmInfo = new(farm);
        farmInfo.portals.Add(fromFarmToHome, homeDown);
        farmInfo.portals.Add(fromFarmToRoad, roadUp);
        farmInfo.portals.Add(fromFarmToMineEntrance, mEDown);

        MapInfo roadInfo = new(road);
        roadInfo.portals.Add(fromRoadToFarm, farmDown);
        roadInfo.portals.Add(fromRoadToVillage, villageLeft);
        roadInfo.portals.Add(fromRoadToBeach, beachUp);

        MapInfo villageInfo = new(village);
        villageInfo.portals.Add(fromVillageToRoad, roadRight);

        MapInfo mineEntranceInfo = new(mineEntrance);
        mineEntranceInfo.portals.Add(fromMEToFarm, farmRight);
        mineEntranceInfo.portals.Add(fromMEToMine, mineCenter);

        MapInfo mineInfo = new(mine);
        mineInfo.portals.Add(fromMineToME, mEUp);

        MapInfo beachInfo = new(beach);
        beachInfo.portals.Add(fromBeachToRoad, roadDown);


        // 매핑위한 딕셔너리 추가
        maps = new Dictionary<MapType, MapInfo>()
        {
            { MapType.Home, homeInfo },
            { MapType.Farm, farmInfo },
            { MapType.Road, roadInfo },
            { MapType.Village, villageInfo },
            { MapType.MineEntrance, mineEntranceInfo },
            { MapType.Mine, mineInfo },
            { MapType.Beach, beachInfo },
        };
    }

    /// <summary>
    /// 플레이어가 이동할 때 타겟 맵 활성화
    /// </summary>
    public void LoadMap(MapType targetType, GameObject entrance)
    {
        PlayerInput input = player.GetComponent<PlayerInput>();
        
        virtualCamera.enabled = false;
        input.enabled = false;

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
            virtualCamera.enabled = true;
        },

        () =>
        {
            input.enabled = true;
        }
        ));
    }

    /// <summary>
    /// 현재 맵 비활성화
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
    /// entrance에 따른 플레이어 스폰 위치 설정
    /// </summary>
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
