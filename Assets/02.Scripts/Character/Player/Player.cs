using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerStat stat;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStat>();
    }

    private void Start()
    {
        playerController.speed = stat.Speed;
    }
}
