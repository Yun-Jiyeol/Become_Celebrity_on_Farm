using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private float ActiveRange;
    Camera camera;

    private void Start()
    {
        camera = gameObject.GetComponent<Player>().MainCamera.GetComponent<Camera>();

        settActiveRange();
    }

    public void settActiveRange()
    {
        ActiveRange = gameObject.GetComponent<Player>().stat.ActiveRange;
    }

    void OnMove(InputValue inputValue)
    {
        dir = inputValue.Get<Vector2>();
    }

    void OnClick(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            Vector2 MousePosition = Input.mousePosition;
            MousePosition = camera.ScreenToWorldPoint(MousePosition);

            Debug.Log(MousePosition);

            if(Vector2.Distance(MousePosition,transform.position) <= ActiveRange)
            {
                Debug.Log("사거리 내");
            }
        }
    }
}
