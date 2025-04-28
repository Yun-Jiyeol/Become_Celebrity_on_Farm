using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobData : MonoBehaviour
{
    [Header("StoneMine 설정")]
    public Tilemap stoneMineGround;
    public Tilemap stoneMineRoad;
    public GameObject[] stoneMobs;

    [Header("CopperMine 설정")]
    public Tilemap copperMineGround;
    public Tilemap copperMineRoad;
    public GameObject[] copperMobs;

    [Header("IronMine 설정")]
    public Tilemap ironMineGround;
    public Tilemap ironMineRoad;
    public GameObject[] ironMobs;

    [Header("Mob 수 설정")]
    public int numberOfStoneMobs = 5;
    public int numberOfCopperMobs = 5;
    public int numberOfIronMobs = 5;

    //[Header("기본 Mob Prefabs (코드로 등록)")]
    public GameObject CloudC;
    public GameObject MobBatSmallB;
    public GameObject EarthSmallerB;
    public GameObject SlimeD;


    void Awake()
    {
        // StoneMine 기본 설정
        if (stoneMobs == null || stoneMobs.Length == 0)
        {
            stoneMobs = new GameObject[] { CloudC, MobBatSmallB, EarthSmallerB, SlimeD };
        }

        // CopperMine 기본 설정 (나중에 추가)
        if (copperMobs == null || copperMobs.Length == 0)
        {
            // 예시: copperMobs = new GameObject[] { CopperMob1, CopperMob2 };
        }

        // IronMine 기본 설정 (나중에 추가)
        if (ironMobs == null || ironMobs.Length == 0)
        {
            // 예시: ironMobs = new GameObject[] { IronMob1, IronMob2 };
        }
    }

    void Start()
    {
        if (stoneMobs != null && stoneMobs.Length > 0)
            SpawnMobs(stoneMineGround, stoneMineRoad, stoneMobs, numberOfStoneMobs);

        //if (copperMobs != null && copperMobs.Length > 0)
        //    SpawnMobs(copperMineGround, copperMineRoad, copperMobs, numberOfCopperMobs);

        //if (ironMobs != null && ironMobs.Length > 0)
        //    SpawnMobs(ironMineGround, ironMineRoad, ironMobs, numberOfIronMobs);
    }

    void SpawnMobs(Tilemap ground, Tilemap road, GameObject[] mobs, int mobCount)
    {
        if (mobs == null || mobs.Length == 0)
        {
            Debug.LogWarning("Mob 프리팹 배열이 비어있습니다!");
            return;
        }

        for (int i = 0; i < mobCount; i++)
        {
            Vector3Int randomPos = GetRandomGroundOrRoadTile(ground, road);
            Vector3 spawnPos = ground.CellToWorld(randomPos) + new Vector3(0.5f, 0.5f, 0);

            GameObject mobPrefab = mobs[Random.Range(0, mobs.Length)];
            Instantiate(mobPrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector3Int GetRandomGroundOrRoadTile(Tilemap ground, Tilemap road)
    {
        Tilemap targetMap = (Random.value > 0.5f) ? ground : road;
        BoundsInt bounds = targetMap.cellBounds;
        List<Vector3Int> validTiles = new List<Vector3Int>();

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (targetMap.HasTile(pos))
                    validTiles.Add(pos);
            }
        }

        if (validTiles.Count == 0)
        {
            Debug.LogWarning("유효한 타일이 없습니다!");
            return Vector3Int.zero;
        }

        return validTiles[Random.Range(0, validTiles.Count)];
    }
}