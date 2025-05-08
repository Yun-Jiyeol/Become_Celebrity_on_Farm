using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingCraft
{
    public GameObject Line;
    public RectTransform Point;
}

public class CraftingScroll : MonoBehaviour
{
    public RectTransform ScrollMain;
    public List<SettingCraft> settingCrafts;
    public CraftTooltip craftTooltip;

    private void Start()
    {
        TestManager.Instance.gameObject.GetComponent<CraftManager>().playerCrafting = this;
    }

    public void AddCraftTable()
    {

    }


    GameObject FindShortestLine()
    {
        float min = settingCrafts[0].Point.position.y;
        GameObject minPoint = settingCrafts[0].Line;

        for(int i =1; i<settingCrafts.Count; i++)
        {
            if (settingCrafts[i].Point.position.y < min)
            {
                min = settingCrafts[i].Point.position.y;
                minPoint = settingCrafts[i].Line;
            }
        }
        return minPoint;
    }

    RectTransform FindLongestLine()
    {
        float max = settingCrafts[0].Point.position.y;
        RectTransform minPoint = settingCrafts[0].Point;

        for (int i = 1; i < settingCrafts.Count; i++)
        {
            if (settingCrafts[i].Point.position.y >= max)
            {
                max = settingCrafts[i].Point.position.y;
                minPoint = settingCrafts[i].Point;
            }
        }
        return minPoint;
    }
}
