using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobData : MonoBehaviour
{
    [Header("StoneMine ����")]
    public Tilemap stoneMineGround;
    public GameObject[] stoneMobs;

    [Header("Mob �� ����")]
    public int numberOfStoneMobs = 5;

    // �⺻ ������ (�̸� �ν����Ϳ� �� �־ Resources���� ã���� ��)
    private string[] defaultMobNames = { "CloudC", "MobBatSmallB", "EarthSmallerB", "SlimeD" };

    void Awake()
    {
        // �ڵ����� Tilemap �Ҵ� (�̸� ���)
        if (stoneMineGround == null)
        {
            GameObject groundObj = GameObject.Find("MineGround");
            if (groundObj != null)
            {
                stoneMineGround = groundObj.GetComponent<Tilemap>();
                if (stoneMineGround == null)
                    Debug.LogWarning("MineGround ������Ʈ�� Tilemap ������Ʈ�� �����ϴ�!");
            }
            else
            {
                Debug.LogWarning("MineGround ������Ʈ�� ã�� �� �����ϴ�!");
            }
        }

        // ���� �迭 ��������� �ڵ� ����
        if (stoneMobs == null || stoneMobs.Length == 0)
        {
            List<GameObject> mobList = new List<GameObject>();
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
            stoneMobs = mobList.ToArray();
        }
    }

    void Start()
    {
        if (stoneMineGround == null || stoneMobs.Length == 0)
        {
            Debug.LogWarning("Tilemap �Ǵ� Mob �������� ��� �־� Mob�� ������ �� �����ϴ�.");
            return;
        }

        SpawnMobs(stoneMineGround, stoneMobs, numberOfStoneMobs);
    }

    void SpawnMobs(Tilemap ground, GameObject[] mobs, int mobCount)
    {
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
        BoundsInt bounds = ground.cellBounds;
        List<Vector3Int> validTiles = new List<Vector3Int>();

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (ground.HasTile(pos) && !usedPositions.Contains(pos))
                {
                    validTiles.Add(pos);
                }
            }
        }

        if (validTiles.Count == 0)
        {
            return Vector3Int.zero;
        }

        Vector3Int selected = validTiles[Random.Range(0, validTiles.Count)];
        usedPositions.Add(selected);
        return selected;
    }
}