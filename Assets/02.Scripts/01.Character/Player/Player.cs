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
    public Inventory inventory;
    public PlayerAnimation playerAnimation;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStats>();
        checkFieldOnMouse = GetComponent<CheckFieldOnMouse>();
        autoGetItem = GetComponentInChildren<PlayerAutoGetItem>();
        sortingOrderGroup = GetComponent<SortingOrderGroup>();
        inventory = GetComponent<Inventory>();
        playerAnimation = GetComponent<PlayerAnimation>();

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        GameManager.Instance.player = gameObject;
        playerController.speed = stat.Speed;
    }

    private void LateUpdate()
    {
        sortingOrderGroup.UpdateSortingOrderGroup();
    }
}
