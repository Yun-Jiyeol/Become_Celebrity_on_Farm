using System.Collections.Generic;
using UnityEngine;
using static Season;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "New SeasonTile", menuName = "Create SeasonTile")]
public class SeasonTileList : ScriptableObject
{
    [System.Serializable]
    public class SeasonTile
    {
        public SeasonType type;
        public TileBase tile;
    }

    public List<SeasonTile> seasonTiles;
    public Dictionary<SeasonType, TileBase> seasonTileDict = new();

    private void OnEnable()
    {
        // SO 생성함과 동시에 계절만 채워넣을 용도
        if (seasonTiles == null)
        {
            seasonTiles = new List<SeasonTile>();
        }
    }

    void RebuildDict()
    {
        seasonTileDict.Clear();
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
        RebuildDict();

        seasonTileDict.TryGetValue(season, out TileBase tile);
        return tile;
    }
}

