using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Store,
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
    public Collider2D cameraBorder;
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
    public LoadingFader fader;

    readonly Dictionary<MapType, Map> mapPair = new();
    MapType targetType;

    CamConfinerChange camConfinerChange;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.player != null);

        camConfinerChange = FindObjectOfType<CamConfinerChange>();

        SetMap();
        UnloadAllMap();
        if (mapPair.TryGetValue(currentMap, out Map curMap))
        {
            curMap.place.SetActive(true);
            SetPlayerPosition(curMap);
            camConfinerChange.ChangeCameraBorder(curMap.cameraBorder);
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
    /// 다음 날로 넘어갈 때 사용
    /// </summary>
    public void RefreshMap()
    {
        UnloadMap(currentMap);
        if (mapPair.TryGetValue(currentMap, out Map curMap))
        {
            curMap.place.SetActive(true);
            SetPlayerPosition(curMap);
            camConfinerChange.ChangeCameraBorder(curMap.cameraBorder);
        }
    }

    /// <summary>
    /// 플레이어가 이동할 때 타겟 맵 활성화
    /// </summary>
    public void MoveMap(MapType targetType, GameObject entrance = null)
    {
        StartCoroutine(fader.Fade(() =>
        {
            // 1. 현재 맵 비활성화
            UnloadMap(currentMap);

            // 2. 타겟 맵 활성화
            if (mapPair.TryGetValue(targetType, out Map targetMap))
            {
                targetMap.place.SetActive(true);
                SetPlayerPosition(targetMap, entrance);
                camConfinerChange.ChangeCameraBorder(targetMap.cameraBorder);
            }

            currentMap = targetType;
        }));
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
        // 1. 입구가 없을 때 기본 스폰포인트로 이동
        if (entrance == null)
        {
            GameManager.Instance.player.transform.position = targetMap.defaultSpawn.position;
            return;
        }
        // 2. 입구가 있을 때 입구에 해당하는 스폰포인트로 이동
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

    public MapType NowPlayerPosition()
    {
        return currentMap;
    }
}
