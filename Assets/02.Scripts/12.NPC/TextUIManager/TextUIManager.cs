// using DG.Tweening.Plugins.Core.PathCore; // ������ �����Ƿ� ����
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; // StringReader�� ����ϱ� ���� �ʿ�
// using UnityEditor; // Editor ��ũ��Ʈ�� �ƴ϶�� �ʿ� �����ϴ�.
using UnityEngine;
// using static ItemDataReader; // ItemDataReader�� static ����� ������� ������ ����
// using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray; // ������ �����Ƿ� ����

// ItemType enum�� ItemDataReader�� �̹� ���ǵǾ� �ִٰ� �����ϰ� ���⼭ �����߽��ϴ�.
// ���� TextUIManager������ �ʿ��ϴٸ� �̰��� �����ϰų�, ���� ���Ϸ� �ű�� ���� �����ϴ�.
// public enum ItemType { /* ... */ }

public enum NPCName
{
    None,
    Blacksmith,
    // ... �ٸ� NPC �̸� �߰� ...
}

// Inspector���� ������ NPC �̸��� CSV ���� Resources ��θ� ��� Ŭ����
[System.Serializable]
public class NPCNameAndPath
{
    public NPCName Name; // NPC �̸�
    // Resources ���� �Ʒ����� �����ϴ� CSV ���� ��� (Ȯ���� ����!)
    // ��: "Additional/Excel/BlacksmithTextDatas"
    public string ResourcesPath;
}

public class NPCTextSave
{
    public string Text;
    public string spriteName; // ��������Ʈ �̸�
    public float MinLike; // �ּ� ȣ���� ����
    public float MaxLike; // �ִ� ȣ���� ����
    public string DailyOrRefeat; // ���� ������� �ݺ� ������� �����ϴ� ���ڿ�
    public string[] Buttons; // ��ȭ ������ ��ư �ؽ�Ʈ �迭
    public bool canClick; // ��ư Ŭ�� ���� ����
    // �ʿ��ϴٸ� �� ��簡 ��Ÿ�� ����(����Ʈ �Ϸ� ���� ��)�� �߰��� �� �ֽ��ϴ�.
}

// ExcelReader �������̽��� ���ǵǾ� �ִٰ� �����մϴ�.
// public interface ExcelReader { /* ... */ }


public class TextUIManager : MonoBehaviour // ItemDataReader�� ������� �����Ƿ� ExcelReader�� ����
{
    // ��ȭâ UI�� �����ϴ� ��ũ��Ʈ ����
    public TextScript TextScript;

    // �ν����Ϳ��� ������ NPC�� CSV ���� ���� ����Ʈ
    [Header("NPC Text CSV Files")]
    public List<NPCNameAndPath> settingnameandpath;

    // private List<StreamReader> reader = new List<StreamReader>(); // StringReader�� ���������� ����� ���̹Ƿ� �ʿ� �����ϴ�.

    // �ε�� NPC �ؽ�Ʈ �����͸� ������ Dictionary
    // Key: NPCName, Value: �ش� NPC�� ��� ��� ��� (NPCTextSave ����Ʈ)
    public Dictionary<NPCName, List<NPCTextSave>> calledNPCText = new Dictionary<NPCName, List<NPCTextSave>>();

    // Singleton ���� (�ʿ��ϴٸ� �� Ŭ������ ����)
    // public static TextUIManager Instance;
    // void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }


    private void Awake()
    {
        // Awake���� ��� NPC �ؽ�Ʈ �����͸� �ε��մϴ�.
        LoadAllNPCTextDataFromCSV();
    }

    // Start �Լ��� �� �������� CSV �ε� �� �Ľ̿� �������� ������ ���� �����Ƿ� �����߽��ϴ�.
    // private void Start()
    // {
    //     // ������ �ε��� Awake���� �̹� �Ϸ�Ǿ����ϴ�.
    // }


    // ��� NPC�� CSV ������ Resources���� �ε��ϰ� �����͸� �Ľ��Ͽ� Dictionary�� �����ϴ� �Լ�
    public void LoadAllNPCTextDataFromCSV() // ���� ReadCSV�� SettingData ������ ��ħ
    {
        // Dictionary �ʱ�ȭ (���� �����Ͱ� �ִٸ� ���)
        calledNPCText.Clear();

        if (settingnameandpath == null || settingnameandpath.Count == 0)
        {
            Debug.LogWarning("TextUIManager�� �ε��� NPC �ؽ�Ʈ CSV ���� ������ �������� �ʾҽ��ϴ�.");
            return;
        }

        // ������ �� NPC�� ���� ������ ��ȸ�ϸ� �ε� �� �Ľ�
        foreach (var npcEntry in settingnameandpath)
        {
            // NPC �̸��̳� ��ΰ� ��ȿ���� ������ �ǳʶݴϴ�.
            if (npcEntry.Name == NPCName.None || string.IsNullOrEmpty(npcEntry.ResourcesPath))
            {
                Debug.LogWarning($"��ȿ���� ���� NPC �ؽ�Ʈ ���� ������ �ֽ��ϴ� (Name: {npcEntry.Name}, Path: '{npcEntry.ResourcesPath}'). �ǳʶݴϴ�.");
                continue;
            }

            // Resources.Load<TextAsset>()�� ����Ͽ� �ش� NPC�� CSV ������ �ε��մϴ�.
            TextAsset csvAsset = Resources.Load<TextAsset>(npcEntry.ResourcesPath);

            // ���� �ε� ���� ���� Ȯ��
            if (csvAsset == null)
            {
                Debug.LogError($"Resources ���� '{npcEntry.ResourcesPath}'���� CSV ������ �ε����� ���߽��ϴ�. ��� �� ���� �̸��� Ȯ�����ּ���.");
                continue; // ���� ���Ϸ� �Ѿ�ϴ�.
            }

            Debug.Log($"CSV ���� '{npcEntry.ResourcesPath}' �ε� ����. {npcEntry.Name} ������ �Ľ� ����.");

            // �ش� NPC �̸����� Dictionary�� ���ο� ����Ʈ�� �߰��մϴ�.
            // ���� ���� �̸��� NPC �����Ͱ� �̹� �ִٸ� ����ϴ�.
            calledNPCText[npcEntry.Name] = new List<NPCTextSave>();

            // TextAsset�� ������ StringReader�� �н��ϴ�.
            using (StringReader reader = new StringReader(csvAsset.text))
            {
                string dataLine;
                // ������ ���� �� �پ� �н��ϴ�.
                while ((dataLine = reader.ReadLine()) != null)
                {
                    // �� ���̳� �ּ� ���� �ǳʶ� �� �ֽ��ϴ�.
                    if (string.IsNullOrWhiteSpace(dataLine) /* || dataLine.StartsWith("#") */)
                    {
                        continue;
                    }

                    // �޸�(,)�� �������� �� ���� �����͸� �и��մϴ�.
                    string[] values = dataLine.Split(',');

                    // ����Ǵ� �� �������� ������ �ش� ���� �����ϰ� ��� ����մϴ�.
                    const int expectedColumnCount = 7; // Text, spriteName, MinLike, MaxLike, DailyOrRefeat, Buttons, canClick
                    if (values.Length < expectedColumnCount)
                    {
                        Debug.LogWarning($"CSV �� �Ľ� ����: '{dataLine}' - ���� �� ���� {expectedColumnCount}�� �� {values.Length}�� �߰�. �� �ǳʶٱ�. ����: {npcEntry.ResourcesPath}");
                        continue;
                    }

                    // --- �Ľ̵� �����͸� NPCTextSave ��ü�� ��� ---
                    NPCTextSave npctext = new NPCTextSave();

                    // �� ���� �����͸� ������ Ÿ������ ��ȯ�Ͽ� NPCTextSave�� �Ҵ��մϴ�.
                    // �����ϰ� �Ľ��ϱ� ���� TryParse�� ����ϴ� ���� �����ϴ�.

                    // Text (string)
                    npctext.Text = values[0].Trim();

                    // spriteName (string)
                    npctext.spriteName = values[1].Trim();

                    // MinLike (float)
                    if (float.TryParse(values[2], out float minLike)) npctext.MinLike = minLike;
                    else Debug.LogWarning($"CSV �Ľ� ����: MinLike '{values[2]}'�� float���� ��ȯ ����. ����: {npcEntry.ResourcesPath}, ��: '{dataLine}'. �⺻�� 0 ���.");

                    // MaxLike (float)
                    if (float.TryParse(values[3], out float maxLike)) npctext.MaxLike = maxLike;
                    else Debug.LogWarning($"CSV �Ľ� ����: MaxLike '{values[3]}'�� float���� ��ȯ ����. ����: {npcEntry.ResourcesPath}, ��: '{dataLine}'. �⺻�� 0 ���.");

                    // DailyOrRefeat (string)
                    npctext.DailyOrRefeat = values[4].Trim();

                    // Buttons (string �迭) - ������(/)�� ���е� ���ڿ��� �и�
                    // �� ���ڿ��̰ų� �����ø� �ִ� ��츦 ó��
                    string buttonsString = values[5].Trim();
                    if (!string.IsNullOrEmpty(buttonsString))
                    {
                        // Split ����� �� ���ڿ��� ���Ե��� �ʵ��� RemoveEmptyEntries �ɼ� ���
                        npctext.Buttons = buttonsString.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        // �� ��ư �ؽ�Ʈ �յ� ���� ����
                        for (int j = 0; j < npctext.Buttons.Length; j++)
                        {
                            npctext.Buttons[j] = npctext.Buttons[j].Trim();
                        }
                    }
                    else
                    {
                        npctext.Buttons = new string[0]; // ��ư�� ���� ��� �� �迭 �Ҵ�
                    }


                    // canClick (bool)
                    // bool.TryParse�� "true" �Ǵ� "false" ���ڿ��� ����� �Ľ��ϹǷ� ����
                    if (bool.TryParse(values[6].Trim(), out bool canClick)) npctext.canClick = canClick;
                    else
                    {
                        Debug.LogWarning($"CSV �Ľ� ����: canClick '{values[6]}'�� bool���� ��ȯ ����. ����: {npcEntry.ResourcesPath}, ��: '{dataLine}'. �⺻�� false ���.");
                        npctext.canClick = false; // �Ľ� ���� �� �⺻�� false �Ҵ�
                    }


                    // --- �Ľ̵� NPCTextSave�� �ش� NPC�� ����Ʈ�� �߰� ---
                    calledNPCText[npcEntry.Name].Add(npctext);
                }
            } // using ���� ����� StringReader�� �ڵ����� �����ϴ�.

            Debug.Log($"{npcEntry.Name} �ؽ�Ʈ ������ �Ľ� �Ϸ�. �� {calledNPCText[npcEntry.Name].Count}���� ��� �ε��.");
        }

        Debug.Log($"��� NPC �ؽ�Ʈ ������ �ε� �Ϸ�. �� {calledNPCText.Count}���� NPC ������ ó����.");
    }

    // �ؽ�Ʈ UI ǥ�� �Լ� (���� �ڵ�� ����)
    // �� �Լ��� LoadAllNPCTextDataFromCSV()�� �Ϸ�� �Ŀ� ȣ��Ǿ�� �մϴ�.
    public void ShowTextUI(NPCName name, int num)
    {
        // Dictionary�� �ش� NPC �����Ͱ� �ִ���, �׸��� �ε����� ��ȿ���� Ȯ���մϴ�.
        if (calledNPCText.ContainsKey(name) && num >= 0 && num < calledNPCText[name].Count)
        {
            NPCTextSave temp = calledNPCText[name][num];

            // ResourceManager.Instance.splits���� ��������Ʈ �̸����� ã�ƿɴϴ�.
            Sprite characterSprite = null;
            if (!string.IsNullOrEmpty(temp.spriteName) && ResourceManager.Instance != null && ResourceManager.Instance.splits != null)
            {
                if (ResourceManager.Instance.splits.TryGetValue(temp.spriteName, out Sprite loadedSprite))
                {
                    characterSprite = loadedSprite;
                }
                else
                {
                    Debug.LogWarning($"��������Ʈ �̸� '{temp.spriteName}'�� �ش��ϴ� ��������Ʈ�� ResourceManager.Instance.splits���� ã�� �� �����ϴ�. NPC: {name}, ��� ��ȣ: {num}.");
                }
            }
            else if (string.IsNullOrEmpty(temp.spriteName))
            {
                Debug.LogWarning($"NPC {name}, ��� ��ȣ {num}�� ��������Ʈ �̸��� ����ֽ��ϴ�.");
            }
            else
            {
                Debug.LogError($"ResourceManager.Instance �Ǵ� splits Dictionary�� ��ȿ���� �ʽ��ϴ�. ��������Ʈ �ε� ����. NPC: {name}, ��� ��ȣ: {num}.");
            }


            // TextScript�� SettingTextScript �Լ� ȣ�� (��������Ʈ�� ���� ��� null ����)
            TextScript.SettingTextScript(temp.Text, characterSprite, temp.Buttons);
            TextScript.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"NPC '{name}' �Ǵ� ��� ��ȣ '{num}'�� �ش��ϴ� �ؽ�Ʈ �����͸� ã�� �� �����ϴ�.");
            // �����͸� ã�� ������ ��� UI�� ��Ȱ��ȭ�ϰų� ���� �޽����� ǥ���ϴ� ���� ó���� �� �� �ֽ��ϴ�.
            if (TextScript != null && TextScript.gameObject.activeSelf)
            {
                TextScript.gameObject.SetActive(false);
            }
        }
    }

    // �� ��ũ��Ʈ�� ���� ������Ʈ�� �ı��� �� Dictionary�� ����ִ� ���� �����ϴ�.
    // private void OnDestroy()
    // {
    //     if (calledNPCText != null)
    //     {
    //         calledNPCText.Clear();
    //         // calledNPCText = null; // �ʿ��ϴٸ� null�� ����
    //     }
    // }
}

// ExcelReader �������̽� ���Ǵ� �״�� �����մϴ�.
// public interface ExcelReader { /* ... */ }

// ResourceManager Ŭ������ splits Dictionary�� �ܺο� ���ǵǾ�� �մϴ�.
// public class ResourceManager : MonoBehaviour { public static ResourceManager Instance; public Dictionary<string, Sprite> splits; /* ... */ }

// TextScript Ŭ������ �ܺο� ���ǵǾ�� �մϴ�.
// public class TextScript : MonoBehaviour { public void SettingTextScript(string text, Sprite sprite, string[] buttons) { /* ... */ } }
