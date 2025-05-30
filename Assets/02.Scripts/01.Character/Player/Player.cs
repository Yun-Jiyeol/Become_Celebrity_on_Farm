using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerController playerController;
    public PlayerStats stat;
    public CheckFieldOnMouse checkFieldOnMouse;
    public SortingOrderGroup sortingOrderGroup;
    public Inventory inventory;
    public PlayerAnimation playerAnimation;

    public PlayerAutoGetItem autoGetItem;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        GameManager.Instance.player = gameObject;

        playerController = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStats>();
        checkFieldOnMouse = GetComponent<CheckFieldOnMouse>();
        sortingOrderGroup = GetComponent<SortingOrderGroup>();
        inventory = GetComponent<Inventory>();
        playerAnimation = GetComponent<PlayerAnimation>();

        autoGetItem = GetComponentInChildren<PlayerAutoGetItem>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        playerController.speed = stat.Speed;

        stat.Name = SceneChangerManager.Instance.GetComponent<PlayerStats>().Name;
        stat.FarmName = SceneChangerManager.Instance.GetComponent<PlayerStats>().FarmName;
        stat.CharacterType = SceneChangerManager.Instance.GetComponent<PlayerStats>().CharacterType;
    }

    private void LateUpdate()
    {
        sortingOrderGroup.UpdateSortingOrderGroup();
    }
}
