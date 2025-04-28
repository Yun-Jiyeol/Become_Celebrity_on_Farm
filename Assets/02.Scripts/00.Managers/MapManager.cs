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
    StoneMine,
    CopperMine,
    IronMine,
    Beach,
}


/// <summary>
/// 입구, 스폰포인트 묶을 클래스
/// </summary>
[System.Serializable]
public class Portal
{
    public GameObject entrance;
    public List<Transform> spawnPoints;
}

/// <summary>
/// 맵 정보를 담을 클래스
/// </summary>
[System.Serializable]
public class Map
{
    public MapType mapType;
    public GameObject place;
    public List<Portal> portals;
    public Transform defaultSpawn;
}

/// <summary>
/// Map을 모아둘 클래스
/// </summary>
[System.Serializable]
public class MapList
{
    public List<Map> maps;
}

/// <summary>
/// 맵 로직 매니저
/// </summary>
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("MapType")]
    [SerializeField] private MapType currentMap;

    [Header("MapInfo")]
    [SerializeField] private List<Map> maps;

    [Header("UI")]
    public MineSelectUI mineSelectUI;

    [Header("Fader")]
    [SerializeField] private LoadingFader fader;

    [Header("Camera")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

    readonly Dictionary<MapType, Map> mapPair = new();
    MapType targetType;


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

        if (mapPair.TryGetValue(currentMap, out Map map))
        {
            map.place.SetActive(true);
            SetPlayerPosition(map);
        }
    }


    /// <summary>
    /// MapType - Map 연결해서 딕셔너리에 저장
    /// </summary>
    void SetMap()
    {
        foreach (Map map in maps)
        {
            mapPair[map.mapType] = map;
        }
    }

    /// <summary>
    /// 플레이어가 이동할 때 타겟 맵 활성화
    /// </summary>
    public void LoadMap(MapType targetType, GameObject entrance = null)
    {
        // 0. Fade 동안 카메라 움직임, input 막기
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = false;
            virtualCamera.enabled = false;
        }

        StartCoroutine(fader.Fade(() =>
        {
            // 1. 현재 맵 비활성화
            UnloadMap(currentMap);

            // 2. 타겟 맵 활성화
            if (mapPair.TryGetValue(targetType, out Map targetMap))
            {
                // 2-1. 타겟 맵 활성화, 플레이어 스폰 위치로 이동
                targetMap.place.SetActive(true);
                SetPlayerPosition(targetMap, entrance);

                // 2-2. 채집 요소 스폰
                StuffSpawner spawner = targetMap.place.GetComponentInChildren<StuffSpawner>();

                if (spawner)
                {
                    //Debug.Log("스포너 찾기 성공");
                    for (int i = 0; i < 100; i++)
                    {
                        spawner.SpawnStuff();
                    }
                }
                //else
                //    Debug.Log("스포너 찾기 실패");
            }

            currentMap = targetType;
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
    void UnloadMap(MapType currentType)
    {
        if (mapPair.TryGetValue(currentType, out Map currentMap))
            currentMap.place.SetActive(false);
    }

    /// <summary>
    /// Start 때 한 번 호출. 전체 맵 비활성화
    /// </summary>
    void UnloadAllMap()
    {
        foreach (KeyValuePair<MapType, Map> map in mapPair)
        {
            map.Value.place.SetActive(false);
        }
    }

    /// <summary>
    /// entrance에 따른 플레이어 스폰 위치 설정
    /// </summary>
    void SetPlayerPosition(Map targetMap, GameObject entrance = null)
    {
        // 1. 해당 맵에서 입구가 없을 때. 스폰포인트만 있다면 리스트 첫 번째로 둘 것
        if (entrance == null)
        {
            GameManager.Instance.player.transform.position = targetMap.defaultSpawn.position;
            return;
        }
        // 2. 해당 맵에서 입구가 있을 때
        else
        {
            if (mapPair.TryGetValue(currentMap, out Map cur))
            {
                foreach (Portal portal in cur.portals)
                {
                    if (portal.entrance == entrance)
                    {
                        // 2-1. 입구 1개 - 스폰포인트 1개
                        if (portal.spawnPoints.Count == 1)
                        {
                            GameManager.Instance.player.transform.position = portal.spawnPoints[0].position;
                        }
                        // 2-2. 입구 1개 - 스폰포인트 n개
                        else
                        {
                            foreach (KeyValuePair<MapType, Map> pair in mapPair)
                            {
                                if (pair.Value == targetMap)
                                {
                                    targetType = pair.Key;
                                    break;
                                }
                            }

                            int index = GetSpawnIndex(targetType);
                            Debug.Log(index);
                            GameManager.Instance.player.transform.position = portal.spawnPoints[index].position;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 입구 1개에 스폰포인트가 여러 개일 때 index 추출
    /// </summary>
    int GetSpawnIndex(MapType selectedType)
    {
        switch (selectedType)
        {
            case MapType.StoneMine:
                return 0;
            case MapType.CopperMine:
                return 1;
            case MapType.IronMine:
                return 2;
            default:
                return 99;
        }
    }
}