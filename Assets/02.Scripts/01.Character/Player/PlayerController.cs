using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    public bool isAction = false;

    public int PlayerChoosNum = 1;
    private int nownum = 0;

    void OnMove(InputValue inputValue)
    {
        if (isAction) return;
        if (UIManager.Instance.InventoryIsOpen()) return;

        dir = inputValue.Get<Vector2>();
    }

    void OnClick(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {

        }
    }

    void OnInventory(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            Debug.Log("EŰ ����");
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ToggleInventoryUI();

                // �κ��丮�� �����ٸ� ���� ����
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
