using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private PlayerAnimation playerAnimation;
    public bool isAction = false;

    public int PlayerChoosNum = 1;
    private int nownum = 1;

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

            Debug.Log(chooseItemType);

            switch (chooseItemType)
            {
                case ItemType.Pickaxe:
                    if (!GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    CheckAngle();
                    isAction = true;
                    GameManager.Instance.player.GetComponent<Player>().playerAnimation.animator.SetTrigger(
                         GameManager.Instance.player.GetComponent<Player>().playerAnimation.HoeParameterHash);
                    break;
                case ItemType.Watering:
                    if (!GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    CheckAngle();
                    isAction = true;
                    GameManager.Instance.player.GetComponent<Player>().playerAnimation.animator.SetTrigger(
                         GameManager.Instance.player.GetComponent<Player>().playerAnimation.WateringParameterHash);
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
        Vector3 tartgetPosition = GameManager.Instance.player.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;
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

    void OnInventory(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            Debug.Log("E키 눌림");
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ToggleInventoryUI();

                // 인벤토리가 켜졌다면 강제 멈춤
                if (UIManager.Instance.InventoryIsOpen())
                {
                    dir = Vector2.zero;
                }
            }
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
            TestManager.Instance.ShowChooseUI(PlayerChoosNum);
        }
    }
}
