using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class IOnStart : MonoBehaviour
{
    public string InGroupOfGameManager;
    private SeedGrow basicSeed;

    private void Awake()
    {
        gameObject.TryGetComponent<SeedGrow>(out basicSeed);
    }

    private void Start()
    {
        GameManager.Instance.CanInteractionObjects[InGroupOfGameManager].Add(gameObject);
        Invoke("LateStart", 0.1f);
    }

    void LateStart()
    {
        if (basicSeed != null)
        {
            basicSeed.HP = basicSeed.MaxHP;
            //basicSeed.CheckGrow();
        }
    }
}
