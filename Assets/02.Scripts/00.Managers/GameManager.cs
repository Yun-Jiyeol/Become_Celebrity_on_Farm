using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
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
