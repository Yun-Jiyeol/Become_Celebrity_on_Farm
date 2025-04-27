using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Season;


/// <summary>
/// 계절에 따른 타일
/// ex) 봄 땅 타일
/// </summary>
[System.Serializable]
public class SeasonTile
{
    public SeasonType type;
    public TileBase tile;
}


/// <summary>
/// 계절에 따른 타일을 묶어둘 리스트 SO 생성
/// ex) 봄/여름/가을/겨울 땅 타일
/// </summary>
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
/// 계절에 따라 타일이 달라지도록 함
/// 각 맵의 Tilemap을 묶어둔 오브젝트에 붙여둘 것
/// </summary>

public class SeasonTileChanger : MonoBehaviour
{
    // tilemap 필요함

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private SeasonTileList tileList;

    Season season;


    void OnEnable()
    {
        season.OnSeasonChanged += OnSeasonChanged;
    }

    void OnDisable()
    {
        season.OnSeasonChanged -= OnSeasonChanged;
    }

    void OnSeasonChanged(SeasonType season)
    {
   
    }
}
