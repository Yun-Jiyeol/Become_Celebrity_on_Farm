using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public GameObject player;
    public Camera camera;
    public GameObject MouseFollower;
    public GameObject PlayerRange;

    private GameObject LastGameObject;

    public List<GameObject> OnActive;
    public List<GameObject> TagOnMouse;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        camera = Camera.main;
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }


    public void TryHandInteract()
    {
        //LastGameObject.GetComponent<SeedGrow>().HandInteract();
    }

    public void SpawnSomething(Vector3 position, GameObject _go)
    {
        GameObject go = Instantiate(_go);
        go.transform.parent = gameObject.transform;
        go.transform.position = position;
    }

    public bool TagIsInMouse(string[] _tag)
    {
        foreach(string tag in _tag)
        {
            if(TagOnMouse.Count == 0) return false;

            for(int i =0;  i< TagOnMouse.Count; i++)
            {
                if(tag == TagOnMouse[i].transform.tag) return true;
            }
        }
        return false;
    }
    public bool TagIsNotInMouse(string[] _tag)
    {
        foreach (string tag in _tag)
        {
            if (TagOnMouse.Count == 0) return true;

            for (int i = 0; i < TagOnMouse.Count; i++)
            {
                if (tag == TagOnMouse[i].transform.tag) return false;
            }
        }
        return true;
    }
    public void TagIsInRange(string[] _tag, int _dir, bool isAll)
    {
        if (TagOnMouse.Count == 0) return;

        for (int i = 0; i < TagOnMouse.Count; i++)
        {
            foreach(string tag in _tag)
            {
                if (TagOnMouse[i].transform.tag == tag)
                {
                    float angleDegrees = Mathf.Atan2(TagOnMouse[i].transform.position.x - player.transform.position.x, TagOnMouse[i].transform.position.y - player.transform.position.y) * Mathf.Rad2Deg;
                    bool isin = false;
                    switch (_dir)
                    {
                        case 0:
                            if (angleDegrees <= 60 && angleDegrees >= -60)
                            {
                                isin = true;
                            }
                            break;
                        case 1:
                            if (angleDegrees <= 150 && angleDegrees >= 30)
                            {
                                isin = true;
                            }
                            break;
                        case 2:
                            if (angleDegrees <= -120 || angleDegrees >= 120)
                            {
                                isin = true;
                            }
                            break;
                        case 3:
                            if (angleDegrees <= -30 && angleDegrees >= -150)
                            {
                                isin = true;
                            }
                            break;
                    }

                    if (isin)
                    {
                        TagOnMouse[i].GetComponent<IInteract>().Interact();
                        if (!isAll) return;
                    }
                }
            }
        }
    }
}
