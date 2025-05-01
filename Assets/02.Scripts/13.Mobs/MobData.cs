using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobData : MonoBehaviour
{
    [Header("StoneMine ����")]
    public Tilemap stoneMineGround;
    public GameObject[] stoneMobs;

    [Header("CopperMine ����")]
    public Tilemap copperMineGround;
    public GameObject[] copperMobs;

    [Header("IronMine ����")]
    public Tilemap ironMineGround;
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
        // StoneMine �ڵ� �Ҵ�
        if (stoneMineGround == null)
        {
            GameObject groundObj = GameObject.FindGameObjectWithTag("StoneMineGround");
            if (groundObj != null)
            {
                stoneMineGround = groundObj.GetComponent<Tilemap>();
                if (stoneMineGround == null)
                    Debug.LogWarning("�±� 'StoneMineGround' ������Ʈ�� Tilemap ������Ʈ�� �����ϴ�!");
            }
            else
            {
                Debug.LogWarning("�±� 'StoneMineGround' ������Ʈ�� ã�� �� �����ϴ�!");
            }
        }

        //CopperMine �ڵ� �Ҵ�
        if (copperMineGround == null)
        {
            GameObject groundObj = GameObject.FindGameObjectWithTag("CopperMineGround");
            if (groundObj != null)
            {
                copperMineGround = groundObj.GetComponent<Tilemap>();
                if (copperMineGround == null)
                    Debug.LogWarning("�±� 'CopperMineGround' ������Ʈ�� Tilemap ������Ʈ�� �����ϴ�!");
            }
            else
            {
                Debug.LogWarning("�±� 'CopperMineGround' ������Ʈ�� ã�� �� �����ϴ�!");
            }
        }

        //IronMine �ڵ� �Ҵ�
        if (ironMineGround == null)
        {
            GameObject groundObj = GameObject.FindGameObjectWithTag("IronMineGround");
            if (groundObj != null)
            {
                ironMineGround = groundObj.GetComponent<Tilemap>();
                if (ironMineGround == null)
                    Debug.LogWarning("�±� 'IronMineGround' ������Ʈ�� Tilemap ������Ʈ�� �����ϴ�!");
            }
            else
            {
                Debug.LogWarning("�±� 'IronMineGround' ������Ʈ�� ã�� �� �����ϴ�!");
            }
        }

        // stoneMobs �迭 �ڵ� ����
        if ((stoneMobs == null || stoneMobs.Length == 0) ||
            (stoneMobs.Length > 0 && stoneMobs[0] == null))
        {
            List<GameObject> mobList = new List<GameObject>();
            string[] defaultMobNames = { "CloudC", "MobBatSmallB", "EarthSmallerB", "SlimeD" };
            foreach (string mobName in defaultMobNames)
            {
                GameObject prefab = Resources.Load<GameObject>(mobName);
                if (prefab != null)
                {
                    mobList.Add(prefab);
                }
                else
                {
                    Debug.LogWarning($"Resources���� '{mobName}' �������� ã�� �� �����ϴ�.");
                }
            }
            if (mobList.Count > 0)
            {
                stoneMobs = mobList.ToArray();
            }
        }
    }

    void Start()
    {
        if (stoneMobs != null && stoneMobs.Length > 0)
            SpawnMobs(stoneMineGround, stoneMobs, numberOfStoneMobs);

        // ���߿� Ȱ��ȭ
        // if (copperMobs != null && copperMobs.Length > 0)
        //     SpawnMobs(copperMineGround, copperMobs, numberOfCopperMobs);

        // if (ironMobs != null && ironMobs.Length > 0)
        //     SpawnMobs(ironMineGround, ironMobs, numberOfIronMobs);
    }

    void SpawnMobs(Tilemap ground, GameObject[] mobs, int mobCount)
    {
        if (mobs == null || mobs.Length == 0)
        {
            Debug.LogWarning("Mob ������ �迭�� ����ֽ��ϴ�!");
            return;
        }

        HashSet<Vector3Int> usedPositions = new HashSet<Vector3Int>();

        for (int i = 0; i < mobCount; i++)
        {
            Vector3Int randomPos = GetRandomGroundTile(ground, usedPositions);

            if (randomPos == Vector3Int.zero)
            {
                Debug.LogWarning("Mob�� ������ ��ȿ�� Ÿ���� �����մϴ�.");
                return;
            }

            Vector3 spawnPos = ground.CellToWorld(randomPos) + new Vector3(0.5f, 0.5f, 0);
            GameObject mobPrefab = mobs[Random.Range(0, mobs.Length)];
            Instantiate(mobPrefab, spawnPos, Quaternion.identity);

            Debug.DrawLine(spawnPos, spawnPos + Vector3.up * 2f, Color.red, 5f, false);
        }
    }

    Vector3Int GetRandomGroundTile(Tilemap ground, HashSet<Vector3Int> usedPositions)
    {
        Tilemap targetMap = ground;
        BoundsInt bounds = targetMap.cellBounds;
        List<Vector3Int> validTiles = new List<Vector3Int>();

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (targetMap.HasTile(pos) && !usedPositions.Contains(pos))
                {
                    validTiles.Add(pos);
                }
            }
        }

        if (validTiles.Count == 0)
        {
            return Vector3Int.zero; // �� �̻� ��ġ�� �� ����
        }

        Vector3Int selected = validTiles[Random.Range(0, validTiles.Count)];
        usedPositions.Add(selected);
        return selected;
    }
}