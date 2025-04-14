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
}
