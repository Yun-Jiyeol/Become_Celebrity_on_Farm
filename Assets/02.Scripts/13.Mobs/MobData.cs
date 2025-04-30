using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobData : MonoBehaviour
{
    [Header("StoneMine 설정")]
    public Tilemap stoneMineGround;
    public GameObject[] stoneMobs;

    [Header("Mob 수 설정")]
    public int numberOfStoneMobs = 5;

    // 기본 프리팹 (미리 인스펙터에 안 넣어도 Resources에서 찾도록 함)
    private string[] defaultMobNames = { "CloudC", "MobBatSmallB", "EarthSmallerB", "SlimeD" };

    void Awake()
    {
        // 자동으로 Tilemap 할당 (이름 기반)
        if (stoneMineGround == null)
        {
            GameObject groundObj = GameObject.Find("MineGround");
            if (groundObj != null)
            {
                stoneMineGround = groundObj.GetComponent<Tilemap>();
                if (stoneMineGround == null)
                    Debug.LogWarning("MineGround 오브젝트에 Tilemap 컴포넌트가 없습니다!");
            }
            else
            {
                Debug.LogWarning("MineGround 오브젝트를 찾을 수 없습니다!");
            }
        }

        // 몬스터 배열 비어있으면 자동 구성
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
                    Debug.LogWarning($"Resources에서 '{mobName}' 프리팹을 찾을 수 없습니다.");
                }
            }
            stoneMobs = mobList.ToArray();
        }
    }

    void Start()
    {
        if (stoneMineGround == null || stoneMobs.Length == 0)
        {
            Debug.LogWarning("Tilemap 또는 Mob 프리팹이 비어 있어 Mob을 스폰할 수 없습니다.");
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
                Debug.LogWarning("Mob을 생성할 유효한 타일이 부족합니다.");
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