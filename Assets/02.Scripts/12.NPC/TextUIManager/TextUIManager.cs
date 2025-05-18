// using DG.Tweening.Plugins.Core.PathCore; // 사용되지 않으므로 제거
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; // StringReader를 사용하기 위해 필요
// using UnityEditor; // Editor 스크립트가 아니라면 필요 없습니다.
using UnityEngine;
// using static ItemDataReader; // ItemDataReader의 static 멤버를 사용하지 않으면 제거
// using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray; // 사용되지 않으므로 제거

// ItemType enum은 ItemDataReader에 이미 정의되어 있다고 가정하고 여기서 제거했습니다.
// 만약 TextUIManager에서만 필요하다면 이곳에 정의하거나, 공용 파일로 옮기는 것이 좋습니다.
// public enum ItemType { /* ... */ }

public enum NPCName
{
    None,
    Blacksmith,
    // ... 다른 NPC 이름 추가 ...
}

// Inspector에서 설정할 NPC 이름과 CSV 파일 Resources 경로를 담는 클래스
[System.Serializable]
public class NPCNameAndPath
{
    public NPCName Name; // NPC 이름
    // Resources 폴더 아래부터 시작하는 CSV 파일 경로 (확장자 제외!)
    // 예: "Additional/Excel/BlacksmithTextDatas"
    public string ResourcesPath;
}

public class NPCTextSave
{
    public string Text;
    public string spriteName; // 스프라이트 이름
    public float MinLike; // 최소 호감도 조건
    public float MaxLike; // 최대 호감도 조건
    public string DailyOrRefeat; // 일일 대사인지 반복 대사인지 구분하는 문자열
    public string[] Buttons; // 대화 선택지 버튼 텍스트 배열
    public bool canClick; // 버튼 클릭 가능 여부
    // 필요하다면 이 대사가 나타날 조건(퀘스트 완료 여부 등)을 추가할 수 있습니다.
}

// ExcelReader 인터페이스가 정의되어 있다고 가정합니다.
// public interface ExcelReader { /* ... */ }


public class TextUIManager : MonoBehaviour // ItemDataReader를 상속하지 않으므로 ExcelReader만 남김
{
    // 대화창 UI를 관리하는 스크립트 참조
    public TextScript TextScript;

    // 인스펙터에서 설정할 NPC별 CSV 파일 정보 리스트
    [Header("NPC Text CSV Files")]
    public List<NPCNameAndPath> settingnameandpath;

    // private List<StreamReader> reader = new List<StreamReader>(); // StringReader를 개별적으로 사용할 것이므로 필요 없습니다.

    // 로드된 NPC 텍스트 데이터를 저장할 Dictionary
    // Key: NPCName, Value: 해당 NPC의 모든 대사 목록 (NPCTextSave 리스트)
    public Dictionary<NPCName, List<NPCTextSave>> calledNPCText = new Dictionary<NPCName, List<NPCTextSave>>();

    // Singleton 패턴 (필요하다면 이 클래스에 구현)
    // public static TextUIManager Instance;
    // void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }


    private void Awake()
    {
        // Awake에서 모든 NPC 텍스트 데이터를 로드합니다.
        LoadAllNPCTextDataFromCSV();
    }

    // Start 함수는 이 예제에서 CSV 로드 및 파싱에 직접적인 역할을 하지 않으므로 제거했습니다.
    // private void Start()
    // {
    //     // 데이터 로딩은 Awake에서 이미 완료되었습니다.
    // }


    // 모든 NPC의 CSV 파일을 Resources에서 로드하고 데이터를 파싱하여 Dictionary에 저장하는 함수
    public void LoadAllNPCTextDataFromCSV() // 기존 ReadCSV와 SettingData 역할을 합침
    {
        // Dictionary 초기화 (기존 데이터가 있다면 비움)
        calledNPCText.Clear();

        if (settingnameandpath == null || settingnameandpath.Count == 0)
        {
            Debug.LogWarning("TextUIManager에 로드할 NPC 텍스트 CSV 파일 정보가 설정되지 않았습니다.");
            return;
        }

        // 설정된 각 NPC별 파일 정보를 순회하며 로드 및 파싱
        foreach (var npcEntry in settingnameandpath)
        {
            // NPC 이름이나 경로가 유효하지 않으면 건너뜁니다.
            if (npcEntry.Name == NPCName.None || string.IsNullOrEmpty(npcEntry.ResourcesPath))
            {
                Debug.LogWarning($"유효하지 않은 NPC 텍스트 설정 정보가 있습니다 (Name: {npcEntry.Name}, Path: '{npcEntry.ResourcesPath}'). 건너뜁니다.");
                continue;
            }

            // Resources.Load<TextAsset>()을 사용하여 해당 NPC의 CSV 파일을 로드합니다.
            TextAsset csvAsset = Resources.Load<TextAsset>(npcEntry.ResourcesPath);

            // 파일 로드 성공 여부 확인
            if (csvAsset == null)
            {
                Debug.LogError($"Resources 폴더 '{npcEntry.ResourcesPath}'에서 CSV 파일을 로드하지 못했습니다. 경로 및 파일 이름을 확인해주세요.");
                continue; // 다음 파일로 넘어갑니다.
            }

            Debug.Log($"CSV 파일 '{npcEntry.ResourcesPath}' 로드 성공. {npcEntry.Name} 데이터 파싱 시작.");

            // 해당 NPC 이름으로 Dictionary에 새로운 리스트를 추가합니다.
            // 만약 같은 이름의 NPC 데이터가 이미 있다면 덮어씁니다.
            calledNPCText[npcEntry.Name] = new List<NPCTextSave>();

            // TextAsset의 내용을 StringReader로 읽습니다.
            using (StringReader reader = new StringReader(csvAsset.text))
            {
                string dataLine;
                // 데이터 줄을 한 줄씩 읽습니다.
                while ((dataLine = reader.ReadLine()) != null)
                {
                    // 빈 줄이나 주석 등은 건너뛸 수 있습니다.
                    if (string.IsNullOrWhiteSpace(dataLine) /* || dataLine.StartsWith("#") */)
                    {
                        continue;
                    }

                    // 콤마(,)를 기준으로 각 열의 데이터를 분리합니다.
                    string[] values = dataLine.Split(',');

                    // 예상되는 열 개수보다 적으면 해당 줄은 무시하고 경고를 출력합니다.
                    const int expectedColumnCount = 7; // Text, spriteName, MinLike, MaxLike, DailyOrRefeat, Buttons, canClick
                    if (values.Length < expectedColumnCount)
                    {
                        Debug.LogWarning($"CSV 줄 파싱 오류: '{dataLine}' - 예상 열 개수 {expectedColumnCount}개 중 {values.Length}개 발견. 줄 건너뛰기. 파일: {npcEntry.ResourcesPath}");
                        continue;
                    }

                    // --- 파싱된 데이터를 NPCTextSave 객체에 담기 ---
                    NPCTextSave npctext = new NPCTextSave();

                    // 각 열의 데이터를 적절한 타입으로 변환하여 NPCTextSave에 할당합니다.
                    // 안전하게 파싱하기 위해 TryParse를 사용하는 것이 좋습니다.

                    // Text (string)
                    npctext.Text = values[0].Trim();

                    // spriteName (string)
                    npctext.spriteName = values[1].Trim();

                    // MinLike (float)
                    if (float.TryParse(values[2], out float minLike)) npctext.MinLike = minLike;
                    else Debug.LogWarning($"CSV 파싱 오류: MinLike '{values[2]}'를 float으로 변환 실패. 파일: {npcEntry.ResourcesPath}, 줄: '{dataLine}'. 기본값 0 사용.");

                    // MaxLike (float)
                    if (float.TryParse(values[3], out float maxLike)) npctext.MaxLike = maxLike;
                    else Debug.LogWarning($"CSV 파싱 오류: MaxLike '{values[3]}'를 float으로 변환 실패. 파일: {npcEntry.ResourcesPath}, 줄: '{dataLine}'. 기본값 0 사용.");

                    // DailyOrRefeat (string)
                    npctext.DailyOrRefeat = values[4].Trim();

                    // Buttons (string 배열) - 슬래시(/)로 구분된 문자열을 분리
                    // 빈 문자열이거나 슬래시만 있는 경우를 처리
                    string buttonsString = values[5].Trim();
                    if (!string.IsNullOrEmpty(buttonsString))
                    {
                        // Split 결과에 빈 문자열이 포함되지 않도록 RemoveEmptyEntries 옵션 사용
                        npctext.Buttons = buttonsString.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        // 각 버튼 텍스트 앞뒤 공백 제거
                        for (int j = 0; j < npctext.Buttons.Length; j++)
                        {
                            npctext.Buttons[j] = npctext.Buttons[j].Trim();
                        }
                    }
                    else
                    {
                        npctext.Buttons = new string[0]; // 버튼이 없는 경우 빈 배열 할당
                    }


                    // canClick (bool)
                    // bool.TryParse는 "true" 또는 "false" 문자열만 제대로 파싱하므로 주의
                    if (bool.TryParse(values[6].Trim(), out bool canClick)) npctext.canClick = canClick;
                    else
                    {
                        Debug.LogWarning($"CSV 파싱 오류: canClick '{values[6]}'를 bool으로 변환 실패. 파일: {npcEntry.ResourcesPath}, 줄: '{dataLine}'. 기본값 false 사용.");
                        npctext.canClick = false; // 파싱 실패 시 기본값 false 할당
                    }


                    // --- 파싱된 NPCTextSave를 해당 NPC의 리스트에 추가 ---
                    calledNPCText[npcEntry.Name].Add(npctext);
                }
            } // using 문을 벗어나면 StringReader가 자동으로 닫힙니다.

            Debug.Log($"{npcEntry.Name} 텍스트 데이터 파싱 완료. 총 {calledNPCText[npcEntry.Name].Count}개의 대사 로드됨.");
        }

        Debug.Log($"모든 NPC 텍스트 데이터 로드 완료. 총 {calledNPCText.Count}명의 NPC 데이터 처리됨.");
    }

    // 텍스트 UI 표시 함수 (기존 코드와 동일)
    // 이 함수는 LoadAllNPCTextDataFromCSV()가 완료된 후에 호출되어야 합니다.
    public void ShowTextUI(NPCName name, int num)
    {
        // Dictionary에 해당 NPC 데이터가 있는지, 그리고 인덱스가 유효한지 확인합니다.
        if (calledNPCText.ContainsKey(name) && num >= 0 && num < calledNPCText[name].Count)
        {
            NPCTextSave temp = calledNPCText[name][num];

            // ResourceManager.Instance.splits에서 스프라이트 이름으로 찾아옵니다.
            Sprite characterSprite = null;
            if (!string.IsNullOrEmpty(temp.spriteName) && ResourceManager.Instance != null && ResourceManager.Instance.splits != null)
            {
                if (ResourceManager.Instance.splits.TryGetValue(temp.spriteName, out Sprite loadedSprite))
                {
                    characterSprite = loadedSprite;
                }
                else
                {
                    Debug.LogWarning($"스프라이트 이름 '{temp.spriteName}'에 해당하는 스프라이트를 ResourceManager.Instance.splits에서 찾을 수 없습니다. NPC: {name}, 대사 번호: {num}.");
                }
            }
            else if (string.IsNullOrEmpty(temp.spriteName))
            {
                Debug.LogWarning($"NPC {name}, 대사 번호 {num}에 스프라이트 이름이 비어있습니다.");
            }
            else
            {
                Debug.LogError($"ResourceManager.Instance 또는 splits Dictionary가 유효하지 않습니다. 스프라이트 로드 실패. NPC: {name}, 대사 번호: {num}.");
            }


            // TextScript의 SettingTextScript 함수 호출 (스프라이트가 없을 경우 null 전달)
            TextScript.SettingTextScript(temp.Text, characterSprite, temp.Buttons);
            TextScript.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"NPC '{name}' 또는 대사 번호 '{num}'에 해당하는 텍스트 데이터를 찾을 수 없습니다.");
            // 데이터를 찾지 못했을 경우 UI를 비활성화하거나 오류 메시지를 표시하는 등의 처리를 할 수 있습니다.
            if (TextScript != null && TextScript.gameObject.activeSelf)
            {
                TextScript.gameObject.SetActive(false);
            }
        }
    }

    // 이 스크립트가 붙은 오브젝트가 파괴될 때 Dictionary를 비워주는 것이 좋습니다.
    // private void OnDestroy()
    // {
    //     if (calledNPCText != null)
    //     {
    //         calledNPCText.Clear();
    //         // calledNPCText = null; // 필요하다면 null로 설정
    //     }
    // }
}

// ExcelReader 인터페이스 정의는 그대로 유지합니다.
// public interface ExcelReader { /* ... */ }

// ResourceManager 클래스와 splits Dictionary는 외부에 정의되어야 합니다.
// public class ResourceManager : MonoBehaviour { public static ResourceManager Instance; public Dictionary<string, Sprite> splits; /* ... */ }

// TextScript 클래스는 외부에 정의되어야 합니다.
// public class TextScript : MonoBehaviour { public void SettingTextScript(string text, Sprite sprite, string[] buttons) { /* ... */ } }
