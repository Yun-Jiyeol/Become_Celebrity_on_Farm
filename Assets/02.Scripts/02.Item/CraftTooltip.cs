using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftTooltip : MonoBehaviour
{
    private RectTransform rect;
    public GameObject ExplainObject;
    public Transform SpawnPosition;

    public RectTransform BGSize;
    public GameObject CanMakeBG;
    public GameObject CantMakeBG;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        rect.anchoredPosition = Input.mousePosition - new Vector3(1300,600,0);
    }

    public bool Setting(List<DropItem> needsItems)
    {
        bool canmake = true;

        DeleteChildOnPoint();

        BGSize.sizeDelta = new Vector2(200,100 * needsItems.Count);
        for (int i = 0; i < needsItems.Count; i++)
        {
            GameObject go = Instantiate(ExplainObject, SpawnPosition);
            go.GetComponentInChildren<Image>().sprite = ItemManager.Instance.itemDataReader.itemsDatas[needsItems[i].SpawnItemNum].Item_sprite;
            TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();

            text.text = needsItems[i].SpawnItemAmount.ToString(); 
            if (GameManager.Instance.player.GetComponent<Player>().inventory.FindItem(needsItems[i].SpawnItemNum, needsItems[i].SpawnItemAmount))
            {
                text.color = Color.black;
            }
            else
            {
                text.color = Color.red;
                canmake = false;
            }
        }
            
        SettingBG(canmake);
        return canmake;
    }

    void SettingBG(bool canmake)
    {
        if (canmake)
        {
            CanMakeBG.SetActive(true);
            CantMakeBG.SetActive(false);
        }
        else
        {
            CanMakeBG.SetActive(false);
            CantMakeBG.SetActive(true);
        }
    }

    void DeleteChildOnPoint()
    {
        if(SpawnPosition == null)
        {
            return;
        }

        int childnum = SpawnPosition.transform.childCount;
        if (childnum == 0) return;

        for(int i = childnum-1; i >= 0; i--)
        {
            Transform child = SpawnPosition.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
