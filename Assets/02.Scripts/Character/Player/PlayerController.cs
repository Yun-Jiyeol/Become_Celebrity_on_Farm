using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    void OnMove(InputValue inputValue)
    {
        dir = inputValue.Get<Vector2>();
    }
}
