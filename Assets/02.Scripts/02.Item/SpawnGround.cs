using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public enum ChangedGround
{
    Plow,
    Watered
}

class SaveInfo
{
    public string _Tag;
    public string _spriteOnResourceManager;
    public int _spriteOrder;
}

public class SpawnGround : ObjectPolling
{
    SaveInfo Plow = new SaveInfo() 
    { 
        _Tag = "Plow",
        _spriteOnResourceManager = "GroundPlow_55",
        _spriteOrder = 10
    };

    SaveInfo Watered = new SaveInfo()
    {
        _Tag = "Watered",
        _spriteOnResourceManager = "GroundPlow_66",
        _spriteOrder = 20
    };

    public void SpawnGrounds(ChangedGround Type, Vector3 spawnposition)
    {
        GameObject go = SpawnOrFindThings();
        go.transform.position = spawnposition;

        if (Type == ChangedGround.Plow)
        {
            go.transform.tag = Plow._Tag;
            go.GetComponent<GroundGetWater>().enabled = true;
            SpriteRenderer _sprite = go.GetComponent<SpriteRenderer>();
            _sprite.sprite = ResourceManager.Instance.splits[Plow._spriteOnResourceManager];
            _sprite.sortingOrder = Plow._spriteOrder;
        }
        else if(Type == ChangedGround.Watered)
        {
            go.transform.tag = Watered._Tag;
            go.GetComponent<GroundGetWater>().enabled = false;
            SpriteRenderer _sprite = go.GetComponent<SpriteRenderer>();
            _sprite.sprite = ResourceManager.Instance.splits[Watered._spriteOnResourceManager];
            _sprite.sortingOrder = Watered._spriteOrder;
        }
    }

    protected override GameObject SpawnOrFindThings()
    {
        GameObject go = FindOffthings();
        if (go == null)
        {
            go = new GameObject("Grounds");
            go.transform.parent = gameObject.transform;
            go.transform.localScale = Vector3.one;
            go.AddComponent<SpriteRenderer>();
            BoxCollider2D boxC = go.AddComponent<BoxCollider2D>();
            boxC.isTrigger = true;
            boxC.size = new Vector2(1,1);
            go.AddComponent<SaveOnGM>().OffThisCollider = true;
            go.AddComponent<GroundGetWater>().enabled = false;
            Things.Add(go);
        }

        go.SetActive(true);
        return go;
    }
}
