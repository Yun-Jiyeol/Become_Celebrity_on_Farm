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

    protected override void Start()
    {
        base.Start();

        TestManager.Instance.ShowChooseUI(PlayerChoosNum);
    }

    void OnMove(InputValue inputValue)
    {
        if(isAction) return;

        dir = inputValue.Get<Vector2>();
    }

    void OnClick(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {

        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown("1"))
        {
            PlayerChoosNum = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            PlayerChoosNum = 2;
        }
        else if (Input.GetKeyDown("3"))
        {
            PlayerChoosNum = 3;
        }
        else if (Input.GetKeyDown("4"))
        {
            PlayerChoosNum = 4;
        }
        else if (Input.GetKeyDown("5"))
        {
            PlayerChoosNum = 5;
        }
        else if (Input.GetKeyDown("6"))
        {
            PlayerChoosNum = 6;
        }
        else if (Input.GetKeyDown("7"))
        {
            PlayerChoosNum = 7;
        }
        else if (Input.GetKeyDown("8"))
        {
            PlayerChoosNum = 8;
        }
        else if (Input.GetKeyDown("9"))
        {
            PlayerChoosNum = 9;
        }
        else if (Input.GetKeyDown("0"))
        {
            PlayerChoosNum = 10;
        }
        else if (Input.GetKeyDown("-"))
        {
            PlayerChoosNum = 11;
        }
        else if (Input.GetKeyDown("="))
        {
            PlayerChoosNum = 12;
        }

        if (PlayerChoosNum != nownum)
        {
            nownum = PlayerChoosNum;
            TestManager.Instance.ShowChooseUI(PlayerChoosNum);
        }
    }
}
