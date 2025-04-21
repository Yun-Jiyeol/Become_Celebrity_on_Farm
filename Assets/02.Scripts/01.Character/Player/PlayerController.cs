using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum PlayerInteractType
{
    Point,
    Range
}

public class PlayerController : BaseController
{
    private PlayerAnimation playerAnimation;
    Vector3 tartgetPosition = new Vector3();
    public bool isAction = false;

    public int PlayerChoosNum = 1;
    private int nownum;
    int DirectionSave = 0;

    [Header("Interact")]
    private bool CanSpawn = false;
    ChangedGround Groundtype = ChangedGround.Plow;
    PlayerInteractType nowInteractType;
    ItemType chooseItemType;
    GameObject PlayerInteractRange;


    private RangeInteract readyRangeInteract = new RangeInteract();
    public class RangeInteract
    {
        public string[] _Tag;
        public int _Dir;
        public bool _isAll;
    }


    protected override void Start()
    {
        base.Start();

        playerAnimation = GetComponent<PlayerAnimation>();

        PlayerInteractRange = Instantiate(GameManager.Instance.PlayerRange);
        PlayerInteractRange.SetActive(false);
        Invoke("lateStart", 0.1f);
    }

    void lateStart()
    {
        ChangeSlot(1);
    }

    protected override void FixedUpdate()
    {
        if (isAction) return;

        base.FixedUpdate();
    }

    void OnMove(InputValue inputValue)
    {
        if (UIManager.Instance.InventoryIsOpen()) return;

        dir = inputValue.Get<Vector2>();
    }

    void OnClick(InputValue inputValue)
    {
        if (isAction) return;

        if (inputValue.isPressed)
        {
            GameManager.Instance.TurnOnAllColliders();
            Invoke("UseSomeThing", 0.1f);
        }
    }

    void UseSomeThing()
    {
        if (nowInteractType == PlayerInteractType.Point)
        {
            switch (chooseItemType)
            {
                case ItemType.Hoe:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    CheckAngle();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.HoeParameterHash);

                    if (GameManager.Instance.TagIsNotInMouse(new string[] { "Plow", "Tree", "Stone", "EndGrow" }))
                    {
                        CanSpawn = true;
                        Groundtype = ChangedGround.Plow;
                    }
                    break;
                case ItemType.Watering:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    CheckAngle();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.WateringParameterHash);

                    if (GameManager.Instance.TagIsInMouse(new string[] { "Plow" }))
                    {
                        if (GameManager.Instance.TagIsNotInMouse(new string[] { "Watered" }))
                        {
                            CanSpawn = true;
                            Groundtype = ChangedGround.Watered;
                        }
                    }
                    break;
                case ItemType.Seed:
                    TryHandInteract();
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    if (GameManager.Instance.TagIsInMouse(new string[] { "Plow" }))
                    {
                        if (GameManager.Instance.TagIsNotInMouse(new string[] { "Seeded", "EndGrow" }))
                        {
                            GameObject ConnectedObejct = TestManager.Instance.FindObject(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                            if (ConnectedObejct != null)
                            {
                                GameObject go = Instantiate(ConnectedObejct);
                                go.transform.parent = GameManager.Instance.transform;
                                go.transform.position = tartgetPosition;
                            }
                        }
                    }
                    break;
                case ItemType.TreeSeed:
                    TryHandInteract();
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    if (GameManager.Instance.TagIsNotInMouse(new string[] { "Plow", "Tree", "EndGrow" , "Stone"}))
                    {
                        GameObject ConnectedObejct = TestManager.Instance.FindObject(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                        if (ConnectedObejct != null)
                        {
                            GameObject go = Instantiate(ConnectedObejct);
                            go.transform.parent = GameManager.Instance.transform;
                            go.transform.position = tartgetPosition;
                        }
                    }
                    break;
                default:
                    TryHandInteract();
                    break;
            }
        }
        else if (nowInteractType == PlayerInteractType.Range)
        {
            PlayerInteractRange.SetActive(true);
            PlayerInteractRange.transform.position = gameObject.transform.position;
            PlayerInteractRange.transform.localScale = Vector3.one * 2.5f; //범위는 아이템에서 가져오기

            switch (chooseItemType)
            {
                case ItemType.Sickle:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    SaveDirextionInfo();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.SickleParameterHash);
                    readyRangeInteract = new RangeInteract()
                    {
                        _Tag = new string[] { "EndGrow" },
                        _Dir = DirectionSave,
                        _isAll = true
                    };
                    break;
                case ItemType.Axe:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    SaveDirextionInfo();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.AxeParameterHash);
                    readyRangeInteract = new RangeInteract()
                    {
                        _Tag = new string[] { "Tree", "EndGrow" },
                        _Dir = DirectionSave,
                        _isAll = false
                    };
                    break;
                case ItemType.Pickaxe:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    SaveDirextionInfo();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.PickaxeParameterHash);
                    readyRangeInteract = new RangeInteract()
                    {
                        _Tag = new string[] { "Stone" },
                        _Dir = DirectionSave,
                        _isAll = false
                    };
                    break;
                default:
                    break;
            }
        }
    }

    void SaveDirextionInfo()
    {
        DirectionSave = 0;
        if (dir == new Vector2(-1, 0))
        {
            DirectionSave = 3;
        }
        else if (dir == new Vector2(1, 0))
        {
            DirectionSave = 1;
        }
        else if (dir == new Vector2(0, 1))
        {
            DirectionSave = 0;
        }
        else
        {
            DirectionSave = 2;
        }
    }

    void TryHandInteract()
    {
        //if (GameManager.Instance.InteractPosition(tartgetPosition, new string[] { "SeededGround", "TreeGround"}, new string[] { "EndGrow", "Tree" },
        //    null, null))
        //{
        //    GameManager.Instance.TryHandInteract();
        //}
    }


    void OnInventory(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ToggleInventoryUI();

                if (UIManager.Instance.InventoryIsOpen())
                {
                    dir = Vector2.zero;
                }
            }
        }
    }
    public void EndAction()
    {
        dir = Vector2.zero;
        isAction = false;
    }

    void CheckAngle()
    {
        float angleDegrees = Mathf.Atan2(tartgetPosition.x - gameObject.transform.position.x, tartgetPosition.y - gameObject.transform.position.y) * Mathf.Rad2Deg;

        if (angleDegrees >= -135 && angleDegrees < -45)
        {
            dir = new Vector2(-1, 0);
        }
        else if (angleDegrees >= -45 && angleDegrees < 45)
        {
            dir = new Vector2(0, 1);
        }
        else if (angleDegrees >= 45 && angleDegrees < 135)
        {
            dir = new Vector2(1, 0);
        }
        else
        {
            dir = new Vector2(0, -1);
        }
    }

    public void RangeInteractObject()
    {
        GameManager.Instance.TagIsInRange(readyRangeInteract._Tag, readyRangeInteract._Dir, readyRangeInteract._isAll);
    }

    public void SpawnObject()
    {
        if (!CanSpawn) return;
        CanSpawn = false;

        ItemManager.Instance.spawnGround.SpawnGrounds(Groundtype, tartgetPosition);
    }

    void TryChangeType(PlayerInteractType type)
    {
        if (nowInteractType == type) return;

        switch (type)
        {
            case PlayerInteractType.Range:
                nowInteractType = PlayerInteractType.Range;

                PlayerInteractRange.gameObject.SetActive(true);
                gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.SetActive(false);
                gameObject.GetComponent<CheckFieldOnMouse>().enabled = false;
                break;

            case PlayerInteractType.Point:
                nowInteractType = PlayerInteractType.Point;

                PlayerInteractRange.gameObject.SetActive(false);
                gameObject.GetComponent<CheckFieldOnMouse>().enabled = true;
                break;
        }
    }


    void OnOneSlot(InputValue inputValue)
    {
        ChangeSlot(1);
    }

    void OnTwoSlot(InputValue inputValue)
    {
        ChangeSlot(2);
    }

    void OnThreeSlot(InputValue inputValue)
    {
        ChangeSlot(3);
    }

    void OnFourSlot(InputValue inputValue)
    {
        ChangeSlot(4);
    }

    void OnFiveSlot(InputValue inputValue)
    {
        ChangeSlot(5);
    }

    void OnSixSlot(InputValue inputValue)
    {
        ChangeSlot(6);
    }

    void OnSevenSlot(InputValue inputValue)
    {
        ChangeSlot(7);
    }

    void OnEightSlot(InputValue inputValue)
    {
        ChangeSlot(8);
    }

    void OnNineSlot(InputValue inputValue)
    {
        ChangeSlot(9);
    }

    void OnTenSlot(InputValue inputValue)
    {
        ChangeSlot(10);
    }

    void OnElevenSlot(InputValue inputValue)
    {
        ChangeSlot(11);
    }

    void OnTwelveSlot(InputValue inputValue)
    {
        ChangeSlot(12);
    }

    void ChangeSlot(int num)
    {
        PlayerChoosNum = num;
        if (PlayerChoosNum != nownum)
        {
            nownum = PlayerChoosNum;
            QuickSlotUIManager.Instance.SelectSlot(PlayerChoosNum - 1);

            if(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num == 0)
            {
                chooseItemType = ItemType.Except;
            }
            else
            {
                chooseItemType = ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Item_Type;
            }

            switch (chooseItemType)
            {
                case ItemType.Except:
                case ItemType.Hoe:
                case ItemType.Watering:
                case ItemType.Seed:
                case ItemType.TreeSeed:
                    TryChangeType(PlayerInteractType.Point);
                    break;

                case ItemType.Sickle:
                case ItemType.Axe:
                case ItemType.Pickaxe:
                    TryChangeType(PlayerInteractType.Range);
                    break;
            }
        }
    }
}
