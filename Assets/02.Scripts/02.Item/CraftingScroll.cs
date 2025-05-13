using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Start()
    {
        TestManager.Instance.gameObject.GetComponent<CraftManager>().PlayerCraftTable = this.gameObject;
        Setting();
    }

    private void OnEnable()
    {
        craftTooltip.SetActive(false);
    }

    public void Setting()
    {
        if (TestManager.Instance.gameObject.GetComponent<CraftManager>().ListOfCraftingTable == null) return;
        DestroyAllInLine();
        StartCoroutine(SpawnCraftTable());
    }

    IEnumerator SpawnCraftTable()
    {
        for (int i = 0; i < TestManager.Instance.gameObject.GetComponent<CraftManager>().ListOfCraftingTable.Count; i++)
        {
            AddCraftTable(TestManager.Instance.gameObject.GetComponent<CraftManager>().ListOfCraftingTable[i]);
            yield return null;
        }

        yield return null;
        ScrollMain.sizeDelta = new Vector2(0,FindLongestLine().anchoredPosition.y * -1);
    }

    void AddCraftTable(GameObject go)
    {
        GameObject shortest = FindShortestLine();
        Instantiate(go, shortest.transform);
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
        float min = settingCrafts[0].Point.anchoredPosition.y;
        GameObject minPoint = settingCrafts[0].Line;

        for(int i =1; i<settingCrafts.Count; i++)
        {
            if (settingCrafts[i].Point.anchoredPosition.y > min)
            {
                min = settingCrafts[i].Point.anchoredPosition.y;
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
            if (settingCrafts[i].Point.position.y <= max)
            {
                max = settingCrafts[i].Point.anchoredPosition.y;
                minPoint = settingCrafts[i].Point;
            }
        }
        return minPoint;
    }
}
