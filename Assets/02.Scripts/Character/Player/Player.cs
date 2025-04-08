using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerStat stat;
    public CheckFieldOnMouse checkFieldOnMouse;

    public GameObject mouseFollower;

    public Camera MainCamera; // �ӽ� ī�޶�(GameManager�� CameraManager���� �޾ƿ� ��)

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStat>();
        checkFieldOnMouse = GetComponent<CheckFieldOnMouse>();
    }

    private void Start()
    {
        playerController.speed = stat.Speed;
    }
}
