using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Sword,

    Axe,
    Pickaxe,
    Shovel,
    FishingRod,

    Sickle,
    Hoe,
    Watering,
    Seed,
    TreeSeed,

    Bow,
    Arrow,
    BowBarrel,

    Staff,
    Cane,

    Helmet,
    Armor,
    Pants,
    Shoes,
    Glove,
    Cloak,

    Neckless,
    Ring,

    Wallet,
    Food,

    Interia,
    Except
}

public class ItemDataReader : MonoBehaviour, ExcelReader
{
    string path = "Resources/Additional/Excel/ItemDatas.csv";
    StreamReader reader;

    public Dictionary<int, ItemsData> itemsDatas = new Dictionary<int, ItemsData>();

    [System.Serializable]
    public class ItemsData
    {
        public int Item_num;
        public string Item_name;
        public string Item_Explain;
        public ItemType Item_Type;
        public int Item_Price;
        public int Item_Overlap;

        public Sprite Item_sprite;

        public float Stamina;
        public float Hp;
        public float Stress;
        public float Damage;
        public float Range;
        public float Speed;

        public string Buff; //버프는 종류를 따로 함수화 할 것
    }

    private void Awake()
    {
        ReadCSV();
    }
    private void Start()
    {
        SettingData();
    }

    public void ReadCSV()
    {
        reader = new StreamReader(Application.dataPath + "/" + path);
    }

    public void SettingData()
    {
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
                Item_num = int.Parse(splitData[0]),
                Item_name = splitData[1],
                Item_Explain = splitData[2],
                Item_Type = (ItemType)Enum.Parse(typeof(ItemType), splitData[3]),
                Item_Price = int.TryParse(splitData[4], out int price) ? price : 0,
                Item_Overlap = int.TryParse(splitData[5], out int overlap) ? overlap : 0,

                Item_sprite = ResourceManager.Instance.splits[splitData[6]],

                Stamina = float.TryParse(splitData[7], out float stamina) ? stamina : 0,
                Hp = float.TryParse(splitData[8], out float hp) ? hp : 0,
                Stress = float.TryParse(splitData[9], out float stress) ? stress : 0,
                Damage = float.TryParse(splitData[10], out float damage) ? damage : 0,
                Range = float.TryParse(splitData[11], out float range) ? range : 0,
                Speed = float.TryParse(splitData[12], out float speed) ? speed : 0,

                Buff = splitData[13]
            };

            itemsDatas.Add(itemsData.Item_num, itemsData);
        }
    }
}
