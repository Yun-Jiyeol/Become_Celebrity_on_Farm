using UnityEngine;
using System.Collections.Generic;
using System.IO; // StringReader�� ����ϱ� ���� �ʿ�
using System; // Enum.TryParse, Convert�� ����ϱ� ���� �ʿ�
// using UnityEditor; // Editor ��ũ��Ʈ�� �ƴ϶�� �ʿ� �����ϴ�.
// using System.Linq; // �ʿ��ϴٸ� �߰�

// ItemType enum�� �״�� ����մϴ�.
public enum ItemType
{
    Sword, Axe, Pickaxe, Shovel, FishingRod,
    Sickle, Hoe, Watering, Seed, TreeSeed,
    Bow, Arrow, BowBarrel, Staff, Cane,
    Helmet, Armor, Pants, Shoes, Glove, Cloak,
    Neckless, Ring, Wallet, Food, Interia, Except
}

// ExcelReader �������̽��� ���ǵǾ� �ִٰ� �����մϴ�.
// public interface ExcelReader { /* ... */ }


public class ItemDataReader : MonoBehaviour // ItemDataReader�� Ŭ���� �̸� ����
{
    // Resources ���� �Ʒ� Additional/Excel ������ �ִ� CSV ������ Resources ��� (Ȯ���� ����!)
    // Path.Combine�� ����Ͽ� ��θ� �����ϰ� ������ ���� �ֽ��ϴ�:
    // private string csvResourcePath = Path.Combine("Additional", "Excel", "ItemDatas");
    private string csvResourcePath = "Additional/Excel/ItemDatas";

    // StreamReader reader; // StringReader�� ����� ���̹Ƿ� �ʿ� �����ϴ�.

    // ������ �����͸� ������ Dictionary (������ ��ȣ -> ������ ������)
    public Dictionary<int, ItemsData> itemsDatas = new Dictionary<int, ItemsData>();

    // ������ ������ ����ü (System.Serializable�� Inspector���� �����ְų� ������ �� ����)
    [System.Serializable]
    public class ItemsData
    {
        public int Item_num;        // ������ ��ȣ (Dictionary Ű)
        public string Item_name;
        public string Item_Explain;
        public ItemType Item_Type;
        public int Item_Price;
        public int Item_Overlap;

        public Sprite Item_sprite; // ������ ��������Ʈ (ResourceManager���� ������ ����)

        public float Stamina;
        public float Hp;
        public float Stress;
        public float Damage;
        public float Range;
        public float Speed;

        public string Buff; // ���� ���� (���ڿ��� ����)
    }

    // Singleton ������ ����Ͽ� ��𼭵� ���� �����ϰ� (�ʿ��ϴٸ� �� Ŭ������ ����)
    // public static ItemDataReader Instance;
    // void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }


    private void Start()
    {
        // Awake���� CSV ������ �ε��ϰ� �Ľ��Ͽ� Dictionary�� ä��ϴ�.
        LoadItemDataFromCSV();
    }

    // Start �Լ��� �� �������� CSV �ε� �� �Ľ̿� �������� ������ ���� �����Ƿ� �����߽��ϴ�.
    // private void Start()
    // {
    //     // ������ �ε��� Awake���� �̹� �Ϸ�Ǿ����ϴ�.
    // }

    // CSV ������ Resources���� �ε��ϰ� �����͸� �Ľ��Ͽ� Dictionary�� �����ϴ� �Լ�
    public void LoadItemDataFromCSV() // �Լ� �̸��� �� ��Ȯ�ϰ� ���� (���� ReadCSV)
    {
        // Resources.Load<TextAsset>()�� ����Ͽ� CSV ������ TextAsset���� �ε��մϴ�.
        TextAsset csvAsset = Resources.Load<TextAsset>(csvResourcePath);

        // �ε尡 �����ߴ��� Ȯ���մϴ�.
        if (csvAsset == null)
        {
            Debug.LogError($"Resources ���� '{csvResourcePath}'���� CSV ������ �ε����� ���߽��ϴ�. ��� �� ���� �̸��� Ȯ�����ּ���.");
            return; // ���� �ε� ���� �� �Լ� ����
        }

        Debug.Log($"CSV ���� '{csvResourcePath}' �ε� ����. ������ �Ľ� ����.");

        // TextAsset�� ������ StringReader�� �н��ϴ�.
        // using ���� ����ϸ� StringReader ��� �� �ڵ����� Dispose �˴ϴ�.
        using (StringReader reader = new StringReader(csvAsset.text))
        {
            // ù ��° ��(���) �а� �ǳʶٱ� (CSV ���Ͽ� ����� �ִٰ� ����)
            // ���� CSV ���Ͽ� ����� ���ٸ� �� �κ��� �����ϼ���.
            string headerLine = reader.ReadLine();
            if (headerLine == null && csvAsset.text.Length > 0) // ���Ͽ� ������ �ִµ� ����� �� �о��� ���
            {
                Debug.LogWarning($"CSV ���� '{csvResourcePath}'�� ù ��(���)�� ���� ���߽��ϴ�.");
                // ù ���� ������ �����̶�� �Ʒ� �������� ù �ٺ��� ó���� ���Դϴ�.
            }
            else if (csvAsset.text.Length == 0) // ���� ������ ������ ����ִ� ���
            {
                Debug.LogWarning($"CSV ���� '{csvResourcePath}'�� ������ ����ֽ��ϴ�.");
                return;
            }


            string dataLine;
            // ������ ���� �� �پ� �н��ϴ�.
            while ((dataLine = reader.ReadLine()) != null)
            {
                // �� ���̳� �ּ�(# ������ �����ϴ� �� ��)�� �ǳʶ� �� �ֽ��ϴ�.
                if (string.IsNullOrWhiteSpace(dataLine) /* || dataLine.StartsWith("#") */)
                {
                    continue;
                }

                // �޸�(,)�� �������� �� ���� �����͸� �и��մϴ�.
                // ������ ��ü�� �޸��� ���ԵǾ� �ְ� ����ǥ ó���� �Ǿ� �ִٸ�
                // �ܼ� split(',')���δ� ������ �߻��� �� ������, CSV �Ľ� ���̺귯���� �ʿ��մϴ�.
                string[] values = dataLine.Split(',');

                // ����Ǵ� �� �������� ������ �ش� ���� �����ϰ� ��� ����մϴ�.
                // ItemsData Ŭ������ �ʵ� ������ ���� �� ���� Ȯ�� (��: 14�� �ʵ�)
                const int expectedColumnCount = 14;
                if (values.Length < expectedColumnCount)
                {
                    Debug.LogWarning($"CSV �� �Ľ� ����: '{dataLine}' - ���� �� ���� {expectedColumnCount}�� �� {values.Length}�� �߰�. �� �ǳʶٱ�.");
                    continue;
                }

                // --- �Ľ̵� �����͸� ItemsData ��ü�� ��� ---
                ItemsData itemsData = new ItemsData();

                // �� ���� �����͸� ������ Ÿ������ ��ȯ�Ͽ� ItemsData�� �Ҵ��մϴ�.
                // �����ϰ� �Ľ��ϱ� ���� TryParse�� ����ϴ� ���� �����ϴ�.
                // TryParse ���� �� �⺻���� �Ҵ��ϰų� ��� ����մϴ�.

                // Item_num (int) - �ʼ� ��, �Ľ� ���� �� �ش� �� �ǳʶٱ�
                if (int.TryParse(values[0], out int itemNum))
                {
                    itemsData.Item_num = itemNum;
                }
                else
                {
                    Debug.LogWarning($"CSV �Ľ� ����: Item_num '{values[0]}'�� int�� ��ȯ ����. �� '{dataLine}' �ǳʶٱ�.");
                    continue; // Item_num �Ľ� ���� �� �ش� �� ����
                }

                // Item_name (string)
                itemsData.Item_name = values[1].Trim(); // �յ� ���� ����

                // Item_Explain (string)
                itemsData.Item_Explain = values[2].Trim();

                // Item_Type (Enum)
                // Enum.TryParse�� ����Ͽ� �����ϰ� ��ȯ�մϴ�. (ItemType enum�� ���ǵǾ� �־�� ��)
                // true�� ��ҹ��� ����
                if (Enum.TryParse(values[3].Trim(), true, out ItemType itemType))
                {
                    itemsData.Item_Type = itemType;
                }
                else
                {
                    Debug.LogWarning($"CSV �Ľ� ����: Item_Type '{values[3]}'�� Enum '{typeof(ItemType).Name}'���� ��ȯ ����. Item {itemsData.Item_num}�� �⺻�� ���.");
                    itemsData.Item_Type = ItemType.Except; // �Ľ� ���� �� ������ �⺻�� �Ҵ� (��: Except)
                }

                // Item_Price (int)
                if (int.TryParse(values[4], out int itemPrice)) itemsData.Item_Price = itemPrice;
                else Debug.LogWarning($"CSV �Ľ� ����: Item_Price '{values[4]}'�� int�� ��ȯ ����. Item {itemsData.Item_num}�� �⺻�� 0 ���.");

                // Item_Overlap (int)
                if (int.TryParse(values[5], out int itemOverlap)) itemsData.Item_Overlap = itemOverlap;
                else Debug.LogWarning($"CSV �Ľ� ����: Item_Overlap '{values[5]}'�� int�� ��ȯ ����. Item {itemsData.Item_num}�� �⺻�� 0 ���.");

                // Item_sprite (Sprite)
                // ResourceManager.Instance.splits���� ��������Ʈ �̸����� ã�ƿɴϴ�.
                // ResourceManager Ŭ������ splits Dictionary�� �ܺο��� ���ǵǾ� �ְ�,
                // ��ũ��Ʈ ���� �������� ResourceManager.Instance�� ��ȿ�ϸ�,
                // splits Dictionary�� �ش� �̸��� ��������Ʈ�� �ִٰ� �����մϴ�.
                string spriteName = values[6].Trim();
                if (!string.IsNullOrEmpty(spriteName) && ResourceManager.Instance != null && ResourceManager.Instance.splits != null)
                {
                    if (ResourceManager.Instance.splits.TryGetValue(spriteName, out Sprite loadedSprite))
                    {
                        itemsData.Item_sprite = loadedSprite;
                    }
                    else
                    {
                        Debug.LogWarning($"CSV �Ľ� ����: ��������Ʈ �̸� '{spriteName}'�� �ش��ϴ� ��������Ʈ�� ResourceManager.Instance.splits���� ã�� �� �����ϴ�. Item {itemsData.Item_num}.");
                    }
                }
                else if (string.IsNullOrEmpty(spriteName))
                {
                    Debug.LogWarning($"CSV �Ľ� ���: Item {itemsData.Item_num}�� ��������Ʈ �̸��� ����ֽ��ϴ�.");
                }
                else
                {
                    Debug.LogError($"CSV �Ľ� ����: ResourceManager.Instance �Ǵ� splits Dictionary�� ��ȿ���� �ʽ��ϴ�. ��������Ʈ �ε� ����. Item {itemsData.Item_num}.");
                }


                // ���� ���� ���� (float)
                if (float.TryParse(values[7], out float stamina)) itemsData.Stamina = stamina; else Debug.LogWarning($"CSV �Ľ� ����: Stamina '{values[7]}' ��ȯ ����. Item {itemsData.Item_num}.");
                if (float.TryParse(values[8], out float hp)) itemsData.Hp = hp; else Debug.LogWarning($"CSV �Ľ� ����: Hp '{values[8]}' ��ȯ ����. Item {itemsData.Item_num}.");
                if (float.TryParse(values[9], out float stress)) itemsData.Stress = stress; else Debug.LogWarning($"CSV �Ľ� ����: Stress '{values[9]}' ��ȯ ����. Item {itemsData.Item_num}.");
                if (float.TryParse(values[10], out float damage)) itemsData.Damage = damage; else Debug.LogWarning($"CSV �Ľ� ����: Damage '{values[10]}' ��ȯ ����. Item {itemsData.Item_num}.");
                if (float.TryParse(values[11], out float range)) itemsData.Range = range; else Debug.LogWarning($"CSV �Ľ� ����: Range '{values[11]}' ��ȯ ����. Item {itemsData.Item_num}.");
                if (float.TryParse(values[12], out float speed)) itemsData.Speed = speed; else Debug.LogWarning($"CSV �Ľ� ����: Speed '{values[12]}' ��ȯ ����. Item {itemsData.Item_num}.");

                // Buff (string)
                itemsData.Buff = values[13].Trim();


                // --- �Ľ̵� ItemsData�� Dictionary�� �߰� ---
                // ���� Item_num�� �̹� �ִ��� Ȯ���Ͽ� �ߺ� �߰� ����
                if (itemsDatas.ContainsKey(itemsData.Item_num))
                {
                    Debug.LogWarning($"CSV ������ ����: Item_num '{itemsData.Item_num}'�� �ߺ��˴ϴ�. '{itemsData.Item_name}' ������ �����ʹ� Dictionary�� �߰����� �ʽ��ϴ�. ù ��° �����͸� ���˴ϴ�.");
                }
                else
                {
                    itemsDatas.Add(itemsData.Item_num, itemsData);
                }
            }
        } // using ���� ����� reader�� �ڵ����� �����ϴ�.

        Debug.Log($"CSV ������ �Ľ� �Ϸ�. �� {itemsDatas.Count}���� ������ ������ �ε��.");
    }

    // ����: Dictionary���� ������ ������ �������� (�ʿ��ϴٸ� �߰�)
    // public ItemsData GetItemData(int itemNumber) { ... }
}

// ItemType enum ���Ǵ� �״�� �����մϴ�.
// public enum ItemType { ... }

// ExcelReader �������̽� ���Ǵ� �״�� �����մϴ�.
// public interface ExcelReader { /* ... */ }

// ResourceManager Ŭ������ splits Dictionary�� �ܺο� ���ǵǾ�� �մϴ�.
// public class ResourceManager : MonoBehaviour { public static ResourceManager Instance; public Dictionary<string, Sprite> splits; /* ... */ }
