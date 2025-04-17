using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [Header("GetComponents")]
    protected Collider collider;
    protected Rigidbody rigidbody;

    [Header("Move")]
    public Vector2 dir;
    public float speed;

    protected virtual void Start()
    {
        collider = gameObject.GetComponent<Collider>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Update()
    {

    }

    void Movement()
    {
        if (dir == Vector2.zero) return;

        dir = dir.normalized * speed * Time.deltaTime;
        transform.position += new Vector3(dir.x, dir.y, 0);
    }
}
