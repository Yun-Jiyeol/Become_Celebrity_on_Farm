using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public RectTransform PlayerPosition;

    private void OnEnable()
    {
        switch (MapManager.Instance.NowPlayerPosition())
        {
            case MapType.Home:
            case MapType.Farm:
                PlayerPosition.anchoredPosition = new Vector3(-380, 75, 0);
                break;
            case MapType.MineEntrance:
            case MapType.StoneMine:
            case MapType.Mine:
            case MapType.CopperMine:
            case MapType.IronMine:
                PlayerPosition.anchoredPosition = new Vector3(300, 140, 0);
                break;
            case MapType.Road:
                PlayerPosition.anchoredPosition = new Vector3(-175, -50, 0);
                break;
            case MapType.Village:
            case MapType.Store:
                PlayerPosition.anchoredPosition = new Vector3(100, -180, 0);
                break;
            case MapType.Beach:
                PlayerPosition.anchoredPosition = new Vector3(-370, -150, 0);
                break;
            default:
                PlayerPosition.anchoredPosition = new Vector3(0, 0, 0);
                break;
        }
    }
}
