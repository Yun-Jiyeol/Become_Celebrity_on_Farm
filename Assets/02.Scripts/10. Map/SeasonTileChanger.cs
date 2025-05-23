using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Season;


/// <summary>
/// 계절에 따라 타일 변경하는 클래스
/// </summary>
public class SeasonTileChanger : MonoBehaviour
{
    [SerializeField] private List<Tilemap> tilemaps;
    public int tilenum;
    readonly Dictionary<TileBase, SeasonTileList> tileDict = new();
    readonly Dictionary<Tilemap, Dictionary<Vector3Int, SeasonTileList>> mapPosDict = new();

    Season season;


    void Start()
    {
        season = TimeManager.Instance.season;

        TileSOMapping();
        MapPosMapping();

        if (season != null)
        {
            ChangeTiles(TimeManager.Instance.season.CurrentSeason);
            season.OnSeasonChanged += ChangeTiles;
        }
    }

    /// <summary>
    /// 타일을 SO에 연결
    /// </summary>
    void TileSOMapping()
    {
        List<SeasonTileList> tileList = new List<SeasonTileList>();
        switch (tilenum)
        {
            case 0:
                tileList = ResourceManager.Instance.FarmtileList;
                break;

            case 1:
                tileList = ResourceManager.Instance.RoadtileList;
                break;

            case 2:
                tileList = ResourceManager.Instance.VillagetileList;
                break;

            case 3:
                tileList = ResourceManager.Instance.BeachtileList;
                break;
        }

        foreach (SeasonTileList so in tileList)
        {
            foreach (var st in so.seasonTiles)
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
