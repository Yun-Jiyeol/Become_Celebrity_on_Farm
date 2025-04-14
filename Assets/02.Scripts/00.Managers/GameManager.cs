using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public GameObject player;
    public Camera camera;
    public GameObject MouseFollower;

    public List<GameObject> CanInteractionObjects = new  List<GameObject>();

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

    public bool InteractPosition(Vector3 position, string TargetType, string nopeType)
    {
        if (TargetType == null)
        {
            foreach (GameObject go in CanInteractionObjects)
            {
                if (go.transform.position == position)
                {
                    return false;
                }
            }
            return true;
        }

        bool isOkay = false;

        foreach(GameObject go in CanInteractionObjects)
        {
            if(go.transform.position == position)
            {
                if(go.transform.tag == nopeType) return false;
                if (go.transform.tag == TargetType)
                {
                    isOkay = true;
                }
            }
        }
        return isOkay;
    }
}
