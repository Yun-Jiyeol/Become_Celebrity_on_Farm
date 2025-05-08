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
}


/// <summary>
/// 계절에 따라 달라질 타일
/// </summary>
public class SeasonTileChanger : MonoBehaviour
{
    [SerializeField] private List<Tilemap> tileMaps;            // 타일맵들
    [SerializeField] private List<SeasonTileList> tileList;     // 타일 SO

    Season season;

    void OnEnable()
    {
        //if (season == null)
        //{
        //    Debug.Log("season null");
        //    season = FindObjectOfType<Season>();
        //    season.OnSeasonChanged += ChangeTiles;
        //    Debug.Log("season not null");

        //}
        //else
        //    Debug.Log("else. season not null");
    }

    //void OnDisable()
    //{
    //    season.OnSeasonChanged -= ChangeTiles;
    //}

 

    void ChangeTiles(SeasonType season)
    {
        //foreach (Tilemap tilemap in tileMaps)
        //{
        //    foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        //    {
        //        TileBase curTile = tilemap.GetTile(pos);

        //        Debug.Log($"[SeasonTileChanger] [{tilemap.name}] {pos} 위치에 {curTile.name}.");
        //    }
        //}







    }
}


