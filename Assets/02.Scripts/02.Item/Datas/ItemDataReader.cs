using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Tool,
    Food,
    Except
}

public class ItemDataReader : MonoBehaviour, ExcelReader
{
    string path = "02.Scripts/02.Item/Datas/ItemDatas.csv";
    StreamReader reader;

    public Dictionary<int, ItemsData> itemsDatas = new Dictionary<int, ItemsData>();

    [System.Serializable]
    public class ItemsData
    {
        public int Item_num;
        public string Item_name;
        public string Item_Explain;
        public ItemType Item_Type;
        public float Item_Price;
        public int Item_Overlap;

        public float Stamina;
        public float Hp;
        public float Stress;
        public float Damage;
        public string Buff; //버프는 종류를 따로 함수화 할 것

        public Sprite Item_sprite;
    }

    private void Awake()
    {
        ReadCSV();
    }
    private void Start()
    {
        ItemManager.Instance.itemDataReader = this;
        SettingData();
    }

    public void ReadCSV()
    {
        reader = new StreamReader(Application.dataPath + "/" + path);
    }

    public void SettingData()
    {
        int i = 0;
        while (true)
        {
            string data = reader.ReadLine();
            if(itemsDatas.Count == 0)
            {
                data = reader.ReadLine();
            }

            if (data == null)
            {
                break;
            }

            var splitData = data.Split(',');

            ItemsData itemsData = new ItemsData
            {
                Item_num = i,
                Item_name = splitData[0],
                Item_Explain = splitData[1],
                Item_Type = (ItemType)Enum.Parse(typeof(ItemType), splitData[2]),
                Item_Price = float.TryParse(splitData[3], out float price) ? price : 0,
                Item_Overlap = int.TryParse(splitData[4], out int overlap) ? overlap : 0,
                Stamina = float.TryParse(splitData[5], out float stamina) ? stamina : 0,
                Hp = float.TryParse(splitData[6], out float hp) ? hp : 0,
                Stress = float.TryParse(splitData[7], out float stress) ? stress : 0,
                Damage = float.TryParse(splitData[8], out float damage) ? damage : 0,
                Buff = splitData[9],
                Item_sprite = Resources.Load<Sprite>(splitData[10])
            };

            itemsDatas.Add(itemsData.Item_num, itemsData);
            i++;
        }
    }
}
