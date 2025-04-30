using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ItemDataReader;

public enum NPCName
{
    BlackSmith
}

[System.Serializable]
public class NPCNameAndPath
{
    public NPCName Name;
    public string Path;
}

public class NPCTextSave
{
    public string Text;
    public string spriteName;
}

public class TextUIManager : MonoBehaviour, ExcelReader
{
    public TextScript TextScript;
    public List<NPCNameAndPath> settingnameandpath;
    List<StreamReader> reader;

    public Dictionary<string, List<NPCTextSave>> calledNPCText = new Dictionary<string, List<NPCTextSave>>();

    private void Awake()
    {
        if(settingnameandpath.Count == 0) return;

        ReadCSV();
    }
    private void Start()
    {
        if (settingnameandpath.Count == 0) return;

        SettingData();
    }

    public void ReadCSV()
    {
        for (int i = 0; i < settingnameandpath.Count; i++)
        {
            reader.Add(new StreamReader(Application.dataPath + "/" + settingnameandpath[i].Path));
        }
    }

    public void SettingData()
    {
        /*for (int i = 0; i < settingnameandpath.Count; i++)
        {
            while (true)
            {
                string data = reader.ReadLine();
                if (itemsDatas.Count == 0)
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
        }*/
    }

    public void ShowTextUI()
    {
        //TextScript.SettingTextScript(±Û, »çÁø);
        TextScript.gameObject.SetActive(true);
    }
}
