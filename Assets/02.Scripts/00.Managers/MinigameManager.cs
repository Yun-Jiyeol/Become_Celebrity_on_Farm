using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public FishingMinigame fishingMinigame;

    private void Awake()
    {
        gameObject.GetComponentInParent<GameManager>().minigameManager = this;

        fishingMinigame = gameObject.GetComponent<FishingMinigame>();
    }
}
