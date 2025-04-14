using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    public bool isAction = false;
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
}
