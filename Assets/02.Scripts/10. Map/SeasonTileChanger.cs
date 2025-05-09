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
    private readonly Dictionary<SeasonType, TileBase> seasonTileDict = new();
    

#if UNITY_EDITOR
    void OnEnable()
    {
        //if (seasonTiles == null || seasonTiles.Count == 0)
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

    private void OnValidate()
    {
        // dictionary에 추가
        foreach (var so in seasonTiles)
        {
            seasonTileDict[so.type] = so.tile;
        }
    }

    public TileBase GetSeasonTile(SeasonType season)
    {
        seasonTileDict.TryGetValue(season, out TileBase tile);
        return tile;
    }
}


/// <summary>
/// 계절에 따라 달라질 타일
/// </summary>
public class SeasonTileChanger : MonoBehaviour
{
    [SerializeField] private List<Tilemap> tileMaps;            // 타일맵들
    [SerializeField] private List<SeasonTileList> tileList;     // 타일 SO

    Season season;

    private void OnEnable()
    {
        season = TimeManager.Instance.season;

        if (season != null)
        {
            season.OnSeasonChanged += ChangeTiles;
            Debug.Log($"[SeasonTileChanger] {tileMaps[0].gameObject.name} NOT null");
        }
        else
            Debug.Log($"[SeasonTileChanger] {tileMaps[0].gameObject.name} null");

        ChangeTiles(season.CurrentSeason);
    }


    void ChangeTiles(SeasonType nextSeason)
    {
        // Debug.Log($"[SeasonTileChanger] Current Season = {nextSeason}");
        
        foreach (Tilemap tilemap in tileMaps)
        {
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                TileBase curTile = tilemap.GetTile(pos);
                bool isChanged = false;

                // 현재 타일이 어느 so에 있는가?
                foreach (SeasonTileList so in tileList)
                {
                    foreach (SeasonTile seasonTile in so.seasonTiles)
                    {
                        if (curTile == seasonTile.tile)
                        {
                            TileBase newTile = so.GetSeasonTile(nextSeason);
                            tilemap.SetTile(pos, newTile);
                            isChanged = true;
                            break;
                        }
                    }
                    if (isChanged) break;
                }
            }
        }
    }
}


