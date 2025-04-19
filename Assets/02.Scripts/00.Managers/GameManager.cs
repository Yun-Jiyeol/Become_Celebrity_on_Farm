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

    private GameObject LastGameObject;

    public Dictionary<string, List<GameObject>> CanInteractionObjects = new Dictionary<string, List<GameObject>>
    {
        { "PlowGround", new List<GameObject>() },
        { "WateredGround", new List<GameObject>() },
        { "SeededGround", new List<GameObject>() },
        { "TreeGround", new List<GameObject>() },
        { "StoneGround", new List<GameObject>() },
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

    public bool InteractPosition(Vector3 position, string[] TargetGameObjects, string[] TargetType, string[] nopeGameObjects , string[] nopeType)
    {
        if(nopeGameObjects != null)
        {
            for (int i = 0; i < nopeGameObjects.Length; i++)
            {
                foreach (GameObject go in CanInteractionObjects[nopeGameObjects[i]])
                {
                    if (go.transform.position == position)
                    {
                        foreach (string type in nopeType)
                        {
                            if (go.transform.tag == type)
                            {
                                return false;
                            }
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
                if(go.transform.position == position)
                {
                    foreach(string type in TargetType)
                    {
                        if(go.transform.tag == type)
                        {
                            LastGameObject = go;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void TryHandInteract()
    {
        //LastGameObject.GetComponent<SeedGrow>().HandInteract();
    }

    public void InteractSector(string[] TargetGameObjects, string[] TargetTags, float Distance, int dir, bool isAll)
    {
        List<GameObject> SaveforInteract = new List<GameObject>();

        foreach (string list in TargetGameObjects)
        {
            if (CanInteractionObjects[list].Count == 0) return;
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
                                if (!isAll)
                                {
                                    go.GetComponent<IInteract>().Interact();
                                    return;
                                }
                                SaveforInteract.Add(go);
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

    public void SpawnSomething(string name, Vector3 position, Sprite sprite,int Order,string tag, string List)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = gameObject.transform;
        go.transform.position = position;
        go.transform.localScale = Vector3.one;
        SpriteRenderer GOSprite = go.AddComponent<SpriteRenderer>();
        GOSprite.sprite = sprite;
        GOSprite.sortingOrder = Order;
        go.transform.tag = tag;

        CanInteractionObjects[List].Add(go);
    }

    public void SpawnSomething(Vector3 position, GameObject _go, string List)
    {
        GameObject go = Instantiate(_go);
        go.transform.parent = gameObject.transform;
        go.transform.position = position;

        CanInteractionObjects[List].Add(go);
    }

    public void OneDayAfter()
    {
        if (CanInteractionObjects["TreeGround"] != null)
        {
            foreach (GameObject tree in CanInteractionObjects["TreeGround"])
            {
                if(tree.transform.tag == "Tree" || tree.transform.tag == "EndGrow")
                {
                    //tree.GetComponent<SeedGrow>().Grow(10);
                }
            }
        }


        if (CanInteractionObjects["SeededGround"] != null)
        {
            foreach (GameObject Seed in CanInteractionObjects["SeededGround"])
            {
                if (Seed.transform.tag == "Seeded")
                {
                    bool isWatered = false;

                    foreach (GameObject WaterGround in CanInteractionObjects["WateredGround"])
                    {
                        if(WaterGround.transform.position == Seed.transform.position)
                        {
                            isWatered = true;
                            CanInteractionObjects["WateredGround"].Remove(WaterGround);
                            Destroy(WaterGround);
                            break;
                        }
                    }

                    if (isWatered)
                    {
                        //Seed.GetComponent<SeedGrow>().Grow(10);
                    }
                    else
                    {
                        //½â´Â °ÔÀÌÁö +1
                    }
                }
            }
        }

        List<GameObject> SaveforInteract = new List<GameObject>();

        if (CanInteractionObjects["WateredGround"] != null)
        {
            foreach (GameObject WaterGround in CanInteractionObjects["WateredGround"])
            {
                Destroy(WaterGround);
            }
            CanInteractionObjects["WateredGround"].Clear();
        }
    }

    public Season.SeasonType nowSeason;
    public void OneSeasonAfter()
    {
        if (CanInteractionObjects["TreeGround"] != null)
        {
            foreach (GameObject tree in CanInteractionObjects["TreeGround"])
            {
                if (tree.transform.tag == "Tree" || tree.transform.tag == "EndGrow")
                {
                    //tree.GetComponent<SeedGrow>().CheckGrow();
                }
            }
        }
    }
}
