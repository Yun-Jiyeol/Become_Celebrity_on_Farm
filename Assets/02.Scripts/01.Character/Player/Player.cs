using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerController playerController;
    public PlayerStats stat;
    public CheckFieldOnMouse checkFieldOnMouse;
    public PlayerAutoGetItem autoGetItem;
    public SortingOrderGroup sortingOrderGroup;

    [Header("Others")]
    public GameObject mouseFollower;

    public Camera MainCamera; // �ӽ� ī�޶�(GameManager�� CameraManager���� �޾ƿ� ��)

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStats>();
        checkFieldOnMouse = GetComponent<CheckFieldOnMouse>();
        autoGetItem = GetComponentInChildren<PlayerAutoGetItem>();
        sortingOrderGroup = GetComponent<SortingOrderGroup>();
    }

    private void Start()
    {
        playerController.speed = stat.Speed;
    }

    private void LateUpdate()
    {
        sortingOrderGroup.UpdateSortingOrderGroup();
    }
}
