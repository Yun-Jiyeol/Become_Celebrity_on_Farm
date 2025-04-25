using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Season;


[System.Serializable]
public class SeasonTile
{
    public SeasonType season;
    public TileBase tile;
}

[CreateAssetMenu(fileName = "SeasonTile", menuName = "Create SeasonTile")]
public class SeasonTileList : ScriptableObject
{
    public List<SeasonTile> seasonTiles;
}



/// <summary>
/// 계절에 따라 달라질 타일
/// </summary>

public class SeasonTileChanger : MonoBehaviour
{
    Season season;
    //List<>

    //OnSeasonChanged

    //void Initialized()
    //{
    //    // 시즌 구독
    //    season.UpdateSeason() -= SeasonChanged;
    //    season.UpdateSeason() += SeasonChanged;
    //}

    void SeasonChanged()
    {

    }


}
