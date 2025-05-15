using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

class MiniGameKeys 
{
    public KeyCode keycode;
    public string shownkey;
}

public class FishingMinigame : MonoBehaviour
{
    public GameObject Key;
    public List<KeyCode> AnswerKeys;
    public List<GameObject> SpawnedObject;

    MiniGameKeys[] miniGameKeys = new MiniGameKeys[]
   {
        new MiniGameKeys { keycode = KeyCode.UpArrow, shownkey = "↑" },    // 위 화살표
        new MiniGameKeys { keycode = KeyCode.DownArrow, shownkey = "↓" },  // 아래 화살표
        new MiniGameKeys { keycode = KeyCode.RightArrow, shownkey = "→" },// 오른쪽 화살표
        new MiniGameKeys { keycode = KeyCode.LeftArrow, shownkey = "←" }   // 왼쪽 화살표
   };

    public void OnAllFishingCollider()
    {
        if (GameManager.Instance.FishingRange == null) return;
        foreach(FishingCollider FC in GameManager.Instance.FishingRange)
        {
            FC.OnCollider();
        }
    }

    public int CheckHookedFish(int trashamount)
    {
        List<int> HookedItemNum = new List<int>();
        HookedItemNum.Add(154);
        List<int> Persentage = new List<int>();
        Persentage.Add(trashamount);
        int Sum = trashamount;

        foreach (GameObject go in GameManager.Instance.TagOnMouse)
        {
            if (go.TryGetComponent<FishingCollider>(out FishingCollider FC))
            {
                foreach (FishingRange _fr in FC.thiscolliderRange)
                {
                    HookedItemNum.Add(_fr.ItemData_num);
                    Persentage.Add(_fr.amount);
                    Sum += _fr.amount;
                }
            }
        }

        int ChoosedNum = Random.Range(0, Sum);

        for (int i = 0; i < Persentage.Count; i++)
        {
            if(ChoosedNum <= Persentage[i])
            {
                return HookedItemNum[i];
            }
            else
            {
                ChoosedNum -= Persentage[i];
            }
        }

        return 0;
    }


    public void OffAllFishingCollider()
    {
        if (GameManager.Instance.FishingRange == null) return;
        foreach (FishingCollider FC in GameManager.Instance.FishingRange)
        {
            FC.OffCollider();
        }
    }



    public void StartMinigame(int num)
    {
        Vector3 CenterPosition = Camera.main.transform.position;
        AnswerKeys = new List<KeyCode>();
        SpawnedObject = new List<GameObject>();

        for (int i =0; i< num; i++)
        {
            MiniGameKeys thisKey = miniGameKeys[Random.Range(0, miniGameKeys.Length)];
            AnswerKeys.Add(thisKey.keycode);
            GameObject go = Instantiate(Key, this.transform);
            go.transform.position = new Vector3(CenterPosition.x - num + 2 * i + 1, CenterPosition.y + 4f,0);
            go.GetComponentInChildren<TextMeshPro>().text = thisKey.shownkey;
            SpawnedObject.Add(go);
        }

        StartCoroutine(FishingMinigameCoroutine());
    }

    IEnumerator FishingMinigameCoroutine()
    {
        float time = 0;
        float Maxtime = 5f;
        int keynum = 0;
        bool isSuccess = false;

        while(time < Maxtime)
        {
            time += Time.deltaTime;

            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(AnswerKeys[keynum]))
                {
                    MakeSound(AnswerKeys[keynum]);
                    SpawnedObject[keynum].SetActive(false);
                    keynum++;
                    if (keynum >= AnswerKeys.Count)
                    {
                        isSuccess = true;
                        break;
                    }
                }
                else
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["PiaWrong"]);
                    isSuccess = false;
                    break;
                }
            }
            yield return null;
        }

        foreach (GameObject go in SpawnedObject)
        {
            Destroy(go);
        }

        GameManager.Instance.player.GetComponent<Player>().playerController.EndFishing(isSuccess);
    }

    void MakeSound(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow:
                AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["PiaUp"]);
                break;
            case KeyCode.DownArrow:
                AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["PiaDown"]);
                break;
            case KeyCode.RightArrow:
                AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["PiaRight"]);
                break;
            case KeyCode.LeftArrow:
                AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["PiaLeft"]);
                break;
        }
    }
}
