using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    public bool isAction = false;

    public int PlayerChoosNum = 1;
    private int nownum = 1;

    void OnMove(InputValue inputValue)
    {
        if (isAction) return;
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
                    dir = Vector2.zero;
                    isAction = true;
                    GameManager.Instance.player.GetComponent<Player>().playerAnimation.animator.SetTrigger(
                         GameManager.Instance.player.GetComponent<Player>().playerAnimation.HoeParameterHash);
                    break;
                case ItemType.Watering:
                    dir = Vector2.zero;
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
        isAction = false;
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
