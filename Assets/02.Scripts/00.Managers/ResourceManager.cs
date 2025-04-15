using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance = null;

    public List<Texture2D> NeedToSplit;
    public Dictionary<string, Sprite> splits = new Dictionary<string, Sprite>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach(Texture2D texture in NeedToSplit)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(texture.name);
            foreach(Sprite sprite in sprites)
            {
                splits.Add(sprite.name, sprite);
            }
        }
    }

    public static ResourceManager Instance
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
}
