using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobData : MonoBehaviour
{
    [Header("StoneMine ����")]
    public Tilemap stoneMineGround;
    public Tilemap stoneMineRoad;
    public GameObject[] stoneMobs;

    [Header("CopperMine ����")]
    public Tilemap copperMineGround;
    public Tilemap copperMineRoad;
    public GameObject[] copperMobs;

    [Header("IronMine ����")]
    public Tilemap ironMineGround;
    public Tilemap ironMineRoad;
    public GameObject[] ironMobs;

    [Header("Mob �� ����")]
    public int numberOfStoneMobs = 5;
    public int numberOfCopperMobs = 5;
    public int numberOfIronMobs = 5;

    //[Header("�⺻ Mob Prefabs (�ڵ�� ���)")]
    public GameObject CloudC;
    public GameObject MobBatSmallB;
    public GameObject EarthSmallerB;
    public GameObject SlimeD;


    void Awake()
    {
        // StoneMine �⺻ ����
        if (stoneMobs == null || stoneMobs.Length == 0)
        {
            stoneMobs = new GameObject[] { CloudC, MobBatSmallB, EarthSmallerB, SlimeD };
        }

        // CopperMine �⺻ ���� (���߿� �߰�)
        if (copperMobs == null || copperMobs.Length == 0)
        {
            // ����: copperMobs = new GameObject[] { CopperMob1, CopperMob2 };
        }

        // IronMine �⺻ ���� (���߿� �߰�)
        if (ironMobs == null || ironMobs.Length == 0)
        {
            // ����: ironMobs = new GameObject[] { IronMob1, IronMob2 };
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
            Debug.LogWarning("Mob ������ �迭�� ����ֽ��ϴ�!");
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
            Debug.LogWarning("��ȿ�� Ÿ���� �����ϴ�!");
            return Vector3Int.zero;
        }

        return validTiles[Random.Range(0, validTiles.Count)];
    }
}