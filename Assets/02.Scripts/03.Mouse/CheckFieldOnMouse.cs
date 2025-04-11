using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFieldOnMouse : MonoBehaviour
{
    GameObject MouseFollower;
    private float ActiveRange;
    Camera camera;

    private void Start()
    {
        camera = GameManager.Instance.camera;
        MouseFollower = Instantiate(GameManager.Instance.MouseFollower);

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
