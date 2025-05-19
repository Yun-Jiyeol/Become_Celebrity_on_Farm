using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFieldOnMouse : MonoBehaviour
{
    public GameObject MouseFollower;
    Camera camera;

    private void Start()
    {
        camera = Camera.main;
        MouseFollower = Instantiate(GameManager.Instance.MouseFollower);
    }

    private void Update()
    {
        Vector2 MousePosition = Input.mousePosition;
        MousePosition = camera.ScreenToWorldPoint(MousePosition);
        Vector3 RoundPosition = new Vector2(Mathf.RoundToInt(MousePosition.x / 1.6f), Mathf.RoundToInt(MousePosition.y / 1.6f));

        if (Vector2.Distance(MousePosition, transform.position) <= gameObject.GetComponent<Player>().stat.ActiveRange * 3)
        {
            MouseFollower.SetActive(true);
            if(MouseFollower.transform.position != RoundPosition)
            {
                MouseFollower.transform.position = RoundPosition * 1.6f;
            }
        }
        else
        {
            MouseFollower.SetActive(false);
        }
    }
}
