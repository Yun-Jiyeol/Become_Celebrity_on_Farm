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
    public GameObject craftTooltip;

    public void Setting()
    {
        if (TestManager.Instance.gameObject.GetComponent<CraftManager>().ListOfCraftingTable == null) return;
        DestroyAllInLine();
        AddCraftTable();
    }

    void AddCraftTable()
    {
        for(int i =0; i< TestManager.Instance.gameObject.GetComponent<CraftManager>().ListOfCraftingTable.Count; i++)
        {
            Instantiate(TestManager.Instance.gameObject.GetComponent<CraftManager>().ListOfCraftingTable[i], FindShortestLine().transform);
        }
    }

    void DestroyAllInLine()
    {
        foreach(SettingCraft set in settingCrafts)
        {
            int childnum = set.Line.transform.childCount;
            if(childnum <= 1) continue;

            for (int i = childnum - 1; i >= 0; i--)
            {
                Transform child = set.Line.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
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
