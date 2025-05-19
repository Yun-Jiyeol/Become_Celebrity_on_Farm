using UnityEngine;
using System.Collections.Generic;
using System.IO; // StringReader를 사용하기 위해 필요
using System; // Enum.TryParse, Convert를 사용하기 위해 필요
// using UnityEditor; // Editor 스크립트가 아니라면 필요 없습니다.
// using System.Linq; // 필요하다면 추가

// ItemType enum은 그대로 사용합니다.
public enum ItemType
{
    Sword, Axe, Pickaxe, Shovel, FishingRod,
    Sickle, Hoe, Watering, Seed, TreeSeed,
    Bow, Arrow, BowBarrel, Staff, Cane,
    Helmet, Armor, Pants, Shoes, Glove, Cloak,
    Neckless, Ring, Wallet, Food, Interia, Except
}

// ExcelReader 인터페이스가 정의되어 있다고 가정합니다.
// public interface ExcelReader { /* ... */ }


public class ItemDataReader : MonoBehaviour // ItemDataReader로 클래스 이름 유지
{
    // Resources 폴더 아래 Additional/Excel 폴더에 있는 CSV 파일의 Resources 경로 (확장자 제외!)
    // Path.Combine을 사용하여 경로를 안전하게 조합할 수도 있습니다:
    // private string csvResourcePath = Path.Combine("Additional", "Excel", "ItemDatas");
    private string csvResourcePath = "Additional/Excel/ItemDatas";

    // StreamReader reader; // StringReader를 사용할 것이므로 필요 없습니다.

    // 아이템 데이터를 저장할 Dictionary (아이템 번호 -> 아이템 데이터)
    public Dictionary<int, ItemsData> itemsDatas = new Dictionary<int, ItemsData>();

    // 아이템 데이터 구조체 (System.Serializable은 Inspector에서 보여주거나 저장할 때 유용)
    [System.Serializable]
    public class ItemsData
    {
        public int Item_num;        // 아이템 번호 (Dictionary 키)
        public string Item_name;
        public string Item_Explain;
        public ItemType Item_Type;
        public int Item_Price;
        public int Item_Overlap;

        public Sprite Item_sprite; // 아이템 스프라이트 (ResourceManager에서 가져올 예정)

        public float Stamina;
        public float Hp;
        public float Stress;
        public float Damage;
        public float Range;
        public float Speed;

        public string Buff; // 버프 종류 (문자열로 저장)
    }

    // Singleton 패턴을 사용하여 어디서든 접근 가능하게 (필요하다면 이 클래스에 구현)
    // public static ItemDataReader Instance;
    // void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }


    private void Start()
    {
        // Awake에서 CSV 파일을 로드하고 파싱하여 Dictionary를 채웁니다.
        LoadItemDataFromCSV();
    }

    // Start 함수는 이 예제에서 CSV 로드 및 파싱에 직접적인 역할을 하지 않으므로 제거했습니다.
    // private void Start()
    // {
    //     // 데이터 로딩은 Awake에서 이미 완료되었습니다.
    // }

    // CSV 파일을 Resources에서 로드하고 데이터를 파싱하여 Dictionary에 저장하는 함수
    public void LoadItemDataFromCSV() // 함수 이름을 더 명확하게 변경 (원래 ReadCSV)
    {
        // Resources.Load<TextAsset>()을 사용하여 CSV 파일을 TextAsset으로 로드합니다.
        TextAsset csvAsset = Resources.Load<TextAsset>(csvResourcePath);

        // 로드가 성공했는지 확인합니다.
        if (csvAsset == null)
        {
            Debug.LogError($"Resources 폴더 '{csvResourcePath}'에서 CSV 파일을 로드하지 못했습니다. 경로 및 파일 이름을 확인해주세요.");
            return; // 파일 로드 실패 시 함수 종료
        }

        Debug.Log($"CSV 파일 '{csvResourcePath}' 로드 성공. 데이터 파싱 시작.");

        // TextAsset의 내용을 StringReader로 읽습니다.
        // using 문을 사용하면 StringReader 사용 후 자동으로 Dispose 됩니다.
        using (StringReader reader = new StringReader(csvAsset.text))
        {
            // 첫 번째 줄(헤더) 읽고 건너뛰기 (CSV 파일에 헤더가 있다고 가정)
            // 만약 CSV 파일에 헤더가 없다면 이 부분을 제거하세요.
            string headerLine = reader.ReadLine();
            if (headerLine == null && csvAsset.text.Length > 0) // 파일에 내용이 있는데 헤더를 못 읽었을 경우
            {
                Debug.LogWarning($"CSV 파일 '{csvResourcePath}'의 첫 줄(헤더)을 읽지 못했습니다.");
                // 첫 줄이 데이터 라인이라면 아래 루프에서 첫 줄부터 처리될 것입니다.
            }
            else if (csvAsset.text.Length == 0) // 파일 내용이 완전히 비어있는 경우
            {
                Debug.LogWarning($"CSV 파일 '{csvResourcePath}'의 내용이 비어있습니다.");
                return;
            }


            string dataLine;
            // 데이터 줄을 한 줄씩 읽습니다.
            while ((dataLine = reader.ReadLine()) != null)
            {
                // 빈 줄이나 주석(# 등으로 시작하는 줄 등)은 건너뛸 수 있습니다.
                if (string.IsNullOrWhiteSpace(dataLine) /* || dataLine.StartsWith("#") */)
                {
                    continue;
                }

                // 콤마(,)를 기준으로 각 열의 데이터를 분리합니다.
                // 데이터 자체에 콤마가 포함되어 있고 따옴표 처리가 되어 있다면
                // 단순 split(',')으로는 문제가 발생할 수 있으며, CSV 파싱 라이브러리가 필요합니다.
                string[] values = dataLine.Split(',');

                // 예상되는 열 개수보다 적으면 해당 줄은 무시하고 경고를 출력합니다.
                // ItemsData 클래스의 필드 개수에 맞춰 열 개수 확인 (예: 14개 필드)
                const int expectedColumnCount = 14;
                if (values.Length < expectedColumnCount)
                {
                    Debug.LogWarning($"CSV 줄 파싱 오류: '{dataLine}' - 예상 열 개수 {expectedColumnCount}개 중 {values.Length}개 발견. 줄 건너뛰기.");
                    continue;
                }

                // --- 파싱된 데이터를 ItemsData 객체에 담기 ---
                ItemsData itemsData = new ItemsData();

                // 각 열의 데이터를 적절한 타입으로 변환하여 ItemsData에 할당합니다.
                // 안전하게 파싱하기 위해 TryParse를 사용하는 것이 좋습니다.
                // TryParse 실패 시 기본값을 할당하거나 경고를 출력합니다.

                // Item_num (int) - 필수 값, 파싱 실패 시 해당 줄 건너뛰기
                if (int.TryParse(values[0], out int itemNum))
                {
                    itemsData.Item_num = itemNum;
                }
                else
                {
                    Debug.LogWarning($"CSV 파싱 오류: Item_num '{values[0]}'를 int로 변환 실패. 줄 '{dataLine}' 건너뛰기.");
                    continue; // Item_num 파싱 실패 시 해당 줄 무시
                }

                // Item_name (string)
                itemsData.Item_name = values[1].Trim(); // 앞뒤 공백 제거

                // Item_Explain (string)
                itemsData.Item_Explain = values[2].Trim();

                // Item_Type (Enum)
                // Enum.TryParse를 사용하여 안전하게 변환합니다. (ItemType enum이 정의되어 있어야 함)
                // true는 대소문자 무시
                if (Enum.TryParse(values[3].Trim(), true, out ItemType itemType))
                {
                    itemsData.Item_Type = itemType;
                }
                else
                {
                    Debug.LogWarning($"CSV 파싱 오류: Item_Type '{values[3]}'를 Enum '{typeof(ItemType).Name}'으로 변환 실패. Item {itemsData.Item_num}의 기본값 사용.");
                    itemsData.Item_Type = ItemType.Except; // 파싱 실패 시 적절한 기본값 할당 (예: Except)
                }

                // Item_Price (int)
                if (int.TryParse(values[4], out int itemPrice)) itemsData.Item_Price = itemPrice;
                else Debug.LogWarning($"CSV 파싱 오류: Item_Price '{values[4]}'를 int로 변환 실패. Item {itemsData.Item_num}의 기본값 0 사용.");

                // Item_Overlap (int)
                if (int.TryParse(values[5], out int itemOverlap)) itemsData.Item_Overlap = itemOverlap;
                else Debug.LogWarning($"CSV 파싱 오류: Item_Overlap '{values[5]}'를 int로 변환 실패. Item {itemsData.Item_num}의 기본값 0 사용.");

                // Item_sprite (Sprite)
                // ResourceManager.Instance.splits에서 스프라이트 이름으로 찾아옵니다.
                // ResourceManager 클래스와 splits Dictionary가 외부에서 정의되어 있고,
                // 스크립트 실행 시점에서 ResourceManager.Instance가 유효하며,
                // splits Dictionary에 해당 이름의 스프라이트가 있다고 가정합니다.
                string spriteName = values[6].Trim();
                if (!string.IsNullOrEmpty(spriteName) && ResourceManager.Instance != null && ResourceManager.Instance.splits != null)
                {
                    if (ResourceManager.Instance.splits.TryGetValue(spriteName, out Sprite loadedSprite))
                    {
                        itemsData.Item_sprite = loadedSprite;
                    }
                    else
                    {
                        Debug.LogWarning($"CSV 파싱 오류: 스프라이트 이름 '{spriteName}'에 해당하는 스프라이트를 ResourceManager.Instance.splits에서 찾을 수 없습니다. Item {itemsData.Item_num}.");
                    }
                }
                else if (string.IsNullOrEmpty(spriteName))
                {
                    Debug.LogWarning($"CSV 파싱 경고: Item {itemsData.Item_num}에 스프라이트 이름이 비어있습니다.");
                }
                else
                {
                    Debug.LogError($"CSV 파싱 오류: ResourceManager.Instance 또는 splits Dictionary가 유효하지 않습니다. 스프라이트 로드 실패. Item {itemsData.Item_num}.");
                }


                // 스탯 관련 값들 (float)
                if (float.TryParse(values[7], out float stamina)) itemsData.Stamina = stamina; else Debug.LogWarning($"CSV 파싱 오류: Stamina '{values[7]}' 변환 실패. Item {itemsData.Item_num}.");
                if (float.TryParse(values[8], out float hp)) itemsData.Hp = hp; else Debug.LogWarning($"CSV 파싱 오류: Hp '{values[8]}' 변환 실패. Item {itemsData.Item_num}.");
                if (float.TryParse(values[9], out float stress)) itemsData.Stress = stress; else Debug.LogWarning($"CSV 파싱 오류: Stress '{values[9]}' 변환 실패. Item {itemsData.Item_num}.");
                if (float.TryParse(values[10], out float damage)) itemsData.Damage = damage; else Debug.LogWarning($"CSV 파싱 오류: Damage '{values[10]}' 변환 실패. Item {itemsData.Item_num}.");
                if (float.TryParse(values[11], out float range)) itemsData.Range = range; else Debug.LogWarning($"CSV 파싱 오류: Range '{values[11]}' 변환 실패. Item {itemsData.Item_num}.");
                if (float.TryParse(values[12], out float speed)) itemsData.Speed = speed; else Debug.LogWarning($"CSV 파싱 오류: Speed '{values[12]}' 변환 실패. Item {itemsData.Item_num}.");

                // Buff (string)
                itemsData.Buff = values[13].Trim();


                // --- 파싱된 ItemsData를 Dictionary에 추가 ---
                // 같은 Item_num이 이미 있는지 확인하여 중복 추가 방지
                if (itemsDatas.ContainsKey(itemsData.Item_num))
                {
                    Debug.LogWarning($"CSV 데이터 오류: Item_num '{itemsData.Item_num}'가 중복됩니다. '{itemsData.Item_name}' 아이템 데이터는 Dictionary에 추가되지 않습니다. 첫 번째 데이터만 사용됩니다.");
                }
                else
                {
                    itemsDatas.Add(itemsData.Item_num, itemsData);
                }
            }
        } // using 문을 벗어나면 reader가 자동으로 닫힙니다.

        Debug.Log($"CSV 데이터 파싱 완료. 총 {itemsDatas.Count}개의 아이템 데이터 로드됨.");
    }

    // 예시: Dictionary에서 아이템 데이터 가져오기 (필요하다면 추가)
    // public ItemsData GetItemData(int itemNumber) { ... }
}

// ItemType enum 정의는 그대로 유지합니다.
// public enum ItemType { ... }

// ExcelReader 인터페이스 정의는 그대로 유지합니다.
// public interface ExcelReader { /* ... */ }

// ResourceManager 클래스와 splits Dictionary는 외부에 정의되어야 합니다.
// public class ResourceManager : MonoBehaviour { public static ResourceManager Instance; public Dictionary<string, Sprite> splits; /* ... */ }
