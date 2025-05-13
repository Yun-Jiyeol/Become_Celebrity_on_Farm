using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    public int SpawnItemNum;
    public int SpawnItemAmount;
}

public class Stones : MonoBehaviour, IHaveHP, IInteract
{
    public float _hp;
    public float HP { get; set; }
    public float MaxHP { get; set; }

    public string Itemspritename;
    public string NowObjectname;
    string nowsprite;

    public DropItem[] dropitems;

    private void Awake()
    {
        HP = _hp;
        MaxHP = _hp;
        nowsprite = Itemspritename + NowObjectname; 
    }

    public void GetDamage(float amount)
    {
        HP += amount;

        if (HP <= 0)
        {
            bool reported = false;

            for (int i =0; i < dropitems.Length; i++)
            {
                //ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[dropitems[i].SpawnItemNum], dropitems[i].SpawnItemAmount, gameObject.transform.position);
                var itemData = ItemManager.Instance.itemDataReader.itemsDatas[dropitems[i].SpawnItemNum];
                string itemName = itemData.Item_name;
                Debug.Log($"[Stone] 드롭된 아이템 이름: {itemName}");

                foreach (var questTarget in QuestManager.Instance.GetActiveQuestTargets())
                {
                    Debug.Log($"[Stone] 퀘스트 타겟: {questTarget}");

                    if (questTarget == itemName && !reported)
                    {
                        QuestManager.Instance.ReportProgress(itemName, 1);
                        Debug.Log($"[Stone] 퀘스트 보고됨: {itemName}");
                        reported = true;
                        break; // 하나만 보고하고 끝냄
                    }
                }
                ItemManager.Instance.spawnItem.DropItem(
                itemData,
                dropitems[i].SpawnItemAmount,
                transform.position
                );
            }
            Destroy(gameObject);
        }
        else if (amount <= 0)
        {
            StartCoroutine(DamageCoroutine());
        }
    }

    public void Interact()
    {
        GetDamage(-(GameManager.Instance.player.GetComponent<Player>().stat.Attack + GameManager.Instance.player.GetComponent<Player>().playerController.ItemDamage));
    }
    IEnumerator DamageCoroutine()
    {
        if (ResourceManager.Instance.splits["Damage_" + NowObjectname] != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits["Damage_" + NowObjectname];
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[nowsprite];
    }
}
