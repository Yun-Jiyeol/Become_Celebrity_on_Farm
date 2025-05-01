using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image NPCimage;
    public GameObject BtnSpawnPosition;
    public GameObject Btn;
    List<GameObject> Btns = new List<GameObject>();

    private void Start()
    {
        TestManager.Instance.gameObject.GetComponent<TextUIManager>().TextScript = this;
        gameObject.SetActive(false);
    }

    public void SettingTextScript(string _text, Sprite _NPCimage, string[] btns)
    {
        if (Btns.Count != 0)
        {
            foreach (GameObject go in Btns)
            {
                Destroy(go);
            }
            Btns.Clear();
        }

        text.text = _text;
        NPCimage.sprite = _NPCimage;
        if (btns[0] != "None")
        {
            SpawnBtn(btns);
        }
    }
    
    void SpawnBtn(string[] btns)
    {
        BtnSpawnPosition.GetComponent<RectTransform>().anchoredPosition = new Vector2(200 , 400);

        for (int i = btns.Length - 1; i >= 0 ; i--)
        {
            GameObject btn = Instantiate(Btn, BtnSpawnPosition.transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = btns[i];
            Btns.Add(btn);

            switch (btns[i])
            {
                case "Shop":
                    btn.GetComponent<Button>().onClick.AddListener(OnClickShopBtn);
                    break;
                case "OFF":
                    btn.GetComponent<Button>().onClick.AddListener(OnClickOffBtn);
                    break;
            }
        }

        BtnSpawnPosition.GetComponent<RectTransform>().DOAnchorPos(new Vector2(475, 400),1f);
    }

    void OnClickOffBtn()
    {
        GameManager.Instance.player.GetComponent<Player>().playerController.isNPCInteract = false;
        gameObject.SetActive(false);
    }
    void OnClickShopBtn()
    {
        TestManager.Instance.shopUIManager.ShowShopUI(TestManager.Instance.shopUIManager.lastshopData);
        gameObject.SetActive(false);
    }
}
