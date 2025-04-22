using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private static TestManager instance = null;

    public InvenSlot[] SlotItem = new InvenSlot[12];

    public Season.SeasonType nowSeason;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //InvokeRepeating("SeasonAfter", 15f, 15f);
        InvokeRepeating("DayAfter", 5f, 5f);
    }

    void DayAfter()
    {
        GameManager.Instance.OneDayAfter();
    }
    void SeasonAfter()
    {
        GameManager.Instance.OneSeasonAfter();
    }


    public static TestManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public void SettingInven()
    {
        foreach(InvenSlot item in SlotItem)
        {
            item.SettingSlotUI();
        }
    }

    public void ShowChooseUI(int num)
    {
        for (int i = 0; i < SlotItem.Length; i++)
        {
            if(i == num - 1)
            {
                SlotItem[i].ChooseObject.SetActive(true);
            }
            else
            {
                SlotItem[i].ChooseObject.SetActive(false);
            }
        }
    }
}
