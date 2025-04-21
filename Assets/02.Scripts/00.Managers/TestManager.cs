using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private static TestManager instance = null;

    public InvenSlot[] SlotItem = new InvenSlot[12];

    public Sprite HoeGround;
    public Sprite WaterGround;

    public Season.SeasonType nowSeason;

    public List<ConnectionBetweenItemObject> connectionBetweenItemObjects;
    [System.Serializable]
    public class ConnectionBetweenItemObject
    {
        public int ItemData_Num;
        public GameObject Object;
    }

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
        InvokeRepeating("SeasonAfter", 15f, 15f);
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

    public GameObject FindObject(int num)
    {
        for(int i = 0; i< connectionBetweenItemObjects.Count; i++)
        {
            if (connectionBetweenItemObjects[i].ItemData_Num == num)
            {
                return connectionBetweenItemObjects[i].Object;
            }
        }
        return null;
    }
}
