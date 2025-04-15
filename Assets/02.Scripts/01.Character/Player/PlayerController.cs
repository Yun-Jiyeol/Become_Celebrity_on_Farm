using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private PlayerAnimation playerAnimation;
    Vector3 tartgetPosition = new Vector3();
    public bool isAction = false;

    public int PlayerChoosNum = 1;
    private int nownum = 1;

    private SpawnInteract readyInteract = new SpawnInteract();
    private bool CanSpawn = false;
    public class SpawnInteract
    {
        public string _name;
        public Sprite _sprite;
        public string _Tag;
        public string _AddList;
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
            if (GameManager.Instance.player.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num == 0) return;

            ItemType chooseItemType = ItemManager.Instance.itemDataReader.
                itemsDatas[GameManager.Instance.player.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Item_Type;

            switch (chooseItemType)
            {
                case ItemType.Hoe:
                    if (!GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;
                    CheckAngle();
                    isAction = true;
                    GameManager.Instance.player.GetComponent<Player>().playerAnimation.animator.SetTrigger(
                         GameManager.Instance.player.GetComponent<Player>().playerAnimation.HoeParameterHash);
                    if (GameManager.Instance.InteractPosition(tartgetPosition, null, null, 
                        new string[] { "PlowGround", "SeededGround", "ExceptObject" }, new string[] { "Plow", "Seeded" })) //순서는 좌표, 찾아볼 List이름들, 찾아볼 Tag, 없어야 할 List들, 없어야 할 Tag
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
                    if (!GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;
                    CheckAngle();
                    isAction = true;
                    GameManager.Instance.player.GetComponent<Player>().playerAnimation.animator.SetTrigger(
                         GameManager.Instance.player.GetComponent<Player>().playerAnimation.WateringParameterHash);
                    if (GameManager.Instance.InteractPosition(tartgetPosition, new string[] { "PlowGround" }, "Plow",
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
                case ItemType.Seed:
                    if (!GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;
                    if (GameManager.Instance.InteractPosition(tartgetPosition, new string[] { "PlowGround" }, "Plow", new string[] { "SeededGround" }, new string[] { "Seeded" }))
                    {
                        GameObject ConnectedObejct = TestManager.Instance.FindObject(GameManager.Instance.player.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                        if (ConnectedObejct != null)
                        {
                            GameManager.Instance.SpawnSomething(tartgetPosition, ConnectedObejct, "SeededGround");
                        }
                    }
                    break;
                case ItemType.Sickle:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    isAction = true;


                    GameManager.Instance.player.GetComponent<Player>().playerAnimation.animator.SetTrigger(
                         GameManager.Instance.player.GetComponent<Player>().playerAnimation.SickleParameterHash);
                    GameManager.Instance.InteractSector(new string[] { "SeededGround" }, new string[] { "EndGrow" }, 5f, 30, 120, false);
                    break;
                default:
                    break;
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
            TestManager.Instance.ShowChooseUI(PlayerChoosNum);
        }
    }
}
