using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Season;


[System.Serializable]
public class SeasonTile
{
    public SeasonType type;
    public TileBase tile;
}

[CreateAssetMenu(fileName = "SeasonTile", menuName = "Create SeasonTile")]
public class SeasonTileList : ScriptableObject
{
    public List<SeasonTile> seasonTiles;
    public Dictionary<SeasonType, TileBase> seasonTileDict = new();
    

#if UNITY_EDITOR
    void OnEnable()
    {
        // SO 생성함과 동시에 계절만 채워넣을 용도
        if (seasonTiles == null)
        {
            seasonTiles = new List<SeasonTile>();
            foreach (SeasonType season in System.Enum.GetValues(typeof(SeasonType)))
            {
                seasonTiles.Add(new SeasonTile() { type = season, tile = null });
            }
        }
    }
#endif


    void OnValidate()
    {
        foreach (SeasonTile so in seasonTiles)
        {
            seasonTileDict[so.type] = so.tile;
        }
    }

    /// <summary>
    /// SO에서 타일만 리턴
    /// </summary>
    public TileBase GetSeasonTile(SeasonType season)
    {
        seasonTileDict.TryGetValue(season, out TileBase tile);
        return tile;
    }
}


/// <summary>
/// 계절에 따라 타일 변경하는 클래스
/// </summary>
public class SeasonTileChanger : MonoBehaviour
{
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private List<SeasonTileList> tileList;
    readonly Dictionary<TileBase, SeasonTileList> tileDict = new();
    readonly Dictionary<Tilemap, Dictionary<Vector3Int, SeasonTileList>> mapPosDict = new();

    Season season;


    void Awake()
    {
        TileSOMapping();
        MapPosMapping();
    }

    void Start()
    {
        season = TimeManager.Instance.season;

        if (season != null)
            season.OnSeasonChanged += ChangeTiles;
    }

    /// <summary>
    /// 타일을 SO에 연결
    /// </summary>
    void TileSOMapping()
    {
        foreach (SeasonTileList so in tileList)
        {
            foreach (SeasonTile st in so.seasonTiles)
            {
                if (st.tile == null) continue;
                tileDict[st.tile] = so;
            }
        }
    }

    /// <summary>
    /// 타일맵 안의 각각의 칸과 SO 연결
    /// </summary>
    void MapPosMapping()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            var posTile = new Dictionary<Vector3Int, SeasonTileList>();

            tilemap.CompressBounds();
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue; 

                TileBase tile = tilemap.GetTile(pos);
                if (tileDict.TryGetValue(tile, out var so))
                    posTile[pos] = so;
            }

            mapPosDict[tilemap] = posTile;
        }
    }

    /// <summary>
    /// 연결된 타일맵 위치, SO 기반으로 계절에 따라 타일 변경
    /// </summary>
    void ChangeTiles(SeasonType nextSeason)
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            if (!mapPosDict.TryGetValue(tilemap, out var posTile)) continue;

            foreach (var kvp in posTile)
            {
                Vector3Int pos = kvp.Key;
                SeasonTileList so = kvp.Value;

                TileBase newTile = so.GetSeasonTile(nextSeason);
                tilemap.SetTile(pos, newTile);
            }
        }
    }
}


