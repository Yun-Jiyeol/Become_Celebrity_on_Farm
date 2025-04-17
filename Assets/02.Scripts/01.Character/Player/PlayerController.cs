using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private PlayerAnimation playerAnimation;
    Vector3 tartgetPosition = new Vector3();
    public bool isAction = false;

    public int PlayerChoosNum = 1;
    private int nownum = 1;
    int DirectionSave = 0;

    [Header("Interact")]
    private SpawnInteract readyInteract = new SpawnInteract();
    private bool CanSpawn = false;
    public class SpawnInteract
    {
        public string _name;
        public Sprite _sprite;
        public string _Tag;
        public string _AddList;
    }

    private RangeInteract readyRangeInteract = new RangeInteract();
    public class RangeInteract
    {
        public string[] _Find;
        public string[] _Tag;
        public float _Range;
        public int _Dir;
        public bool _isAll;
    }


    protected override void Start()
    {
        base.Start();

        playerAnimation = GetComponent<PlayerAnimation>();  
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
            if (gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num == 0) return;

            ItemType chooseItemType = ItemManager.Instance.itemDataReader.
                itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Item_Type;

            switch (chooseItemType)
            {
                case ItemType.Hoe:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;
                    CheckAngle();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(
                         gameObject.GetComponent<Player>().playerAnimation.HoeParameterHash);
                    //순서는 좌표, 찾아볼 List이름들, 찾아볼 Tag, 없어야 할 List들, 없어야 할 Tag
                    if (GameManager.Instance.InteractPosition(tartgetPosition, null, null, 
                        new string[] { "PlowGround", "TreeGround", "ExceptObject" }, new string[] { "Plow", "Tree" }))
                    {
                        CanSpawn = true;
                        readyInteract = new SpawnInteract
                        {
                            _name = "PlowGround",
                            _sprite = TestManager.Instance.HoeGround,
                            _Tag = "Plow",
                            _AddList = "PlowGround"
                        };
                    }
                    break;
                case ItemType.Watering:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;
                    CheckAngle();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(
                         gameObject.GetComponent<Player>().playerAnimation.WateringParameterHash);
                    if (GameManager.Instance.InteractPosition(tartgetPosition, new string[] { "PlowGround" }, new string[] { "Plow" } ,
                        new string[] { "WateredGround" }, new string[] { "Watered" }))
                    {
                        CanSpawn = true;
                        readyInteract = new SpawnInteract
                        {
                            _name = "WaterGround",
                            _sprite = TestManager.Instance.WaterGround,
                            _Tag = "Watered",
                            _AddList = "WateredGround"
                        };
                    }
                    break;
                case ItemType.Sickle:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    SaveDirextionInfo();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.SickleParameterHash);
                    readyRangeInteract = new RangeInteract()
                    {
                        _Find = new string[] { "SeededGround" },
                        _Tag = new string[] { "EndGrow" },
                        _Range = 5f,
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
                        _Find = new string[] { "TreeGround" },
                        _Tag = new string[] { "Tree", "EndGrow" },
                        _Range = 2f,
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
                        _Find = new string[] { "StoneGround" },
                        _Tag = new string[] { "Stone" },
                        _Range = 2f,
                        _Dir = DirectionSave,
                        _isAll = false
                    };
                    break;
                case ItemType.Seed:
                    TryHandInteract();
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;
                    if (GameManager.Instance.InteractPosition(tartgetPosition, new string[] { "PlowGround" }, new string[] { "Plow" } , new string[] { "SeededGround" }, new string[] { "Seeded", "EndGrow" }))
                    {
                        GameObject ConnectedObejct = TestManager.Instance.FindObject(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                        if (ConnectedObejct != null)
                        {
                            GameManager.Instance.SpawnSomething(tartgetPosition, ConnectedObejct, "SeededGround");
                        }
                    }
                    break;
                case ItemType.TreeSeed:
                    TryHandInteract();
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;
                    if (GameManager.Instance.InteractPosition(tartgetPosition, null, null,
                        new string[] { "PlowGround", "TreeGround", "ExceptObject" }, new string[] { "Plow", "Tree", "EndGrow" }))
                    {
                        GameObject ConnectedObejct = TestManager.Instance.FindObject(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                        if (ConnectedObejct != null)
                        {
                            GameManager.Instance.SpawnSomething(tartgetPosition, ConnectedObejct, "TreeGround");
                        }
                    }
                        break;
                default:
                    TryHandInteract();
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
        if (GameManager.Instance.InteractPosition(tartgetPosition, new string[] { "SeededGround", "TreeGround"}, new string[] { "EndGrow", "Tree" },
            null, null))
        {
            GameManager.Instance.TryHandInteract();
        }
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
        GameManager.Instance.InteractSector(readyRangeInteract._Find, readyRangeInteract._Tag, readyRangeInteract._Range, readyRangeInteract._Dir, readyRangeInteract._isAll);
    }

    public void SpawnObject()
    {
        if (!CanSpawn) return;
        CanSpawn = false;

        GameManager.Instance.SpawnSomethine(readyInteract._name, tartgetPosition, readyInteract._sprite, readyInteract._Tag, readyInteract._AddList);
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
        }
    }
}
