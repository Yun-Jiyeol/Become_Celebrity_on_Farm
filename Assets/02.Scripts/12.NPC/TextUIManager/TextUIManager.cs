using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ItemDataReader;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public enum NPCName
{
    Blacksmith
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
    List<StreamReader> reader = new List<StreamReader>();

    public Dictionary<NPCName, List<NPCTextSave>> calledNPCText = new Dictionary<NPCName, List<NPCTextSave>>();

    private void Awake()
    {
        if (settingnameandpath.Count == 0) return;

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
        for (int i = 0; i < reader.Count; i++)
        {
            calledNPCText[settingnameandpath[i].Name] = new List<NPCTextSave>();

            while (true)
            {
                string data = reader[i].ReadLine(); 
                if (data == null)
                {
                    break;
                }

                var splitData = data.Split(',');

                NPCTextSave _npctext = new NPCTextSave
                {
                    Text = splitData[0],
                    spriteName = splitData[1]
                };

                calledNPCText[settingnameandpath[i].Name].Add(_npctext);
            }
        }
    }

    public void ShowTextUI(NPCName name, int num)
    {
        NPCTextSave temp = calledNPCText[name][num];
        TextScript.SettingTextScript(temp.Text, ResourceManager.Instance.splits[temp.spriteName]);
        TextScript.gameObject.SetActive(true);
    }

    public void OffTextUI()
    {
        TextScript.gameObject.SetActive(false);
    }
}
