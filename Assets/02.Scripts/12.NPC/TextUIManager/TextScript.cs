using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image NPCimage;

    private void Start()
    {
        TestManager.Instance.gameObject.GetComponent<TextUIManager>().TextScript = this;
        gameObject.SetActive(false);
    }

    public void SettingTextScript(string _text, Sprite _NPCimage)
    {
        text.text = _text;
        NPCimage.sprite = _NPCimage;
    }
}
