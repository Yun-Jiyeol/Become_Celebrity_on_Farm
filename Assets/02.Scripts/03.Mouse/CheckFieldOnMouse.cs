using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFieldOnMouse : MonoBehaviour
{
    public GameObject OnMouseObject;
    GameObject MouseFollower;

    private float ActiveRange;
    Camera camera;

    private void Start()
    {
        camera = gameObject.GetComponent<Player>().MainCamera.GetComponent<Camera>();
        MouseFollower = gameObject.GetComponent<Player>().mouseFollower;

        settActiveRange();
    }

    public void settActiveRange()
    {
        ActiveRange = gameObject.GetComponent<Player>().stat.ActiveRange * 3;
    }

    private void Update()
    {
        Vector2 MousePosition = Input.mousePosition;
        MousePosition = camera.ScreenToWorldPoint(MousePosition);
        Vector3 RoundPosition = new Vector2(Mathf.RoundToInt(MousePosition.x), Mathf.RoundToInt(MousePosition.y));

        if (Vector2.Distance(MousePosition, transform.position) <= ActiveRange)
        {
            MouseFollower.SetActive(true);
            if(MouseFollower.transform.position != RoundPosition)
            {
                MouseFollower.transform.position = RoundPosition;
            }
        }
        else
        {
            MouseFollower.SetActive(false);
        }
    }
}
