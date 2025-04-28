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
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private List<SeasonTileList> tileList;

    Season season;

    //void Initialized()
    //{
    //    season.UpdateSeason() -= SeasonChanged;
    //    season.UpdateSeason() += SeasonChanged;
    //}


    void OnEnable()
    {
        season.OnSeasonChanged += ChangeTiles;
    }

    void OnDisable()
    {
        season.OnSeasonChanged -= ChangeTiles;
    }

    /// <summary>
    /// 계절이 바뀔 때 타일 교체
    /// </summary>
    /// <param name="season"></param>
    void ChangeTiles(SeasonType season)
    {
        // 1. 현재 계절에 맞는 타일 찾기




       

    }
}
