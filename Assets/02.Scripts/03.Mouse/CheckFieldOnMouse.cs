using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFieldOnMouse : MonoBehaviour
{
    public GameObject MouseFollower;
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

        if (Vector2.Distance(MousePosition, transform.position) <= ActiveRange)
        {
            Vector3 RoundPosition = new Vector2(Mathf.RoundToInt(MousePosition.x / 1.6f), Mathf.RoundToInt(MousePosition.y / 1.6f));

            MouseFollower.SetActive(true);
            if(MouseFollower.transform.position != RoundPosition * 1.6f)
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
