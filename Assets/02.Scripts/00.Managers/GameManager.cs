using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public GameObject player;
    public Camera camera;
    public GameObject MouseFollower;

    public Dictionary<string, List<GameObject>> CanInteractionObjects = new Dictionary<string, List<GameObject>>
    {
        { "PlowGround", new List<GameObject>() },
        { "WateredGround", new List<GameObject>() },
        { "SeededGround", new List<GameObject>() },
        { "ExceptObject", new List<GameObject>() }
    };

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

    public bool InteractPosition(Vector3 position, string[] TargetGameObjects, string TargetType, string[] nopeGameObjects , string[] nopeType)
    {
        for (int i = 0; i < nopeGameObjects.Length; i++)
        {
            foreach (GameObject go in CanInteractionObjects[nopeGameObjects[i]])
            {
                if (go.transform.position == position)
                {
                    foreach(string type in nopeType)
                    {
                        if(go.transform.tag == type)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        if(TargetType == null) return true;
        
        foreach(string list in TargetGameObjects)
        {
            foreach(GameObject go in CanInteractionObjects[list])
            {
                if(go.transform.position == position && go.transform.tag == TargetType)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void InteractSector(string[] TargetGameObjects, string[] TargetTags, float Distance, int dir, bool isOne)
    {
        List<GameObject> SaveforInteract = new List<GameObject>();

        foreach (string list in TargetGameObjects)
        {
            foreach (GameObject go in CanInteractionObjects[list])
            {
                if(Vector3.Distance(go.transform.position, player.transform.position) <= Distance)
                {
                    float angleDegrees = Mathf.Atan2(go.transform.position.x - player.transform.position.x, go.transform.position.y - player.transform.position.y) * Mathf.Rad2Deg;
                    bool isin = false;
                    switch (dir)
                    {
                        case 0:
                            if(angleDegrees <= 60 && angleDegrees >= -60)
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

                    if(isin)
                    {
                        foreach (string tag in TargetTags)
                        {
                            if (go.transform.tag == tag)
                            {
                                SaveforInteract.Add(go);
                                if (isOne)
                                {
                                    Debug.Log("123");
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        if(SaveforInteract.Count != 0)
        {
            foreach(GameObject go in SaveforInteract)
            {
                go.GetComponent<IInteract>().Interact();
            }
        }
    }

    public void SpawnSomethine(string name, Vector3 position, Sprite sprite, string tag, string List)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = GameManager.Instance.gameObject.transform;
        go.transform.position = position;
        go.transform.localScale = Vector3.one * 0.625f;
        go.AddComponent<SpriteRenderer>().sprite = sprite;
        go.transform.tag = tag;

        CanInteractionObjects[List].Add(go);
    }

    public void SpawnSomething(Vector3 position, GameObject _go, string List)
    {
        GameObject go = Instantiate(_go);
        go.transform.parent = GameManager.Instance.gameObject.transform;
        go.transform.position = position;

        CanInteractionObjects[List].Add(go);
    }
}
