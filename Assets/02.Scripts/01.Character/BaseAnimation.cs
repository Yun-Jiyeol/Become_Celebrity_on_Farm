using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimation : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected string DirectionParameterName = "Direction";
    protected string WalkParameterName = "Walk";

    protected Vector2 Dir;

    public int DirectionParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }

    protected virtual void Initalize()
    {
        DirectionParameterHash = Animator.StringToHash(DirectionParameterName);
        WalkParameterHash = Animator.StringToHash(WalkParameterName);
    }

    protected virtual void Update()
    {
        SettingDir();
        IsWalk();
    }

    void IsWalk()
    {
        if (Dir == Vector2.zero) animator.SetBool(WalkParameterHash, false);
        else animator.SetBool(WalkParameterHash, true);
    }

    void SettingDir()
    {
        if (Dir.y < 0)
        {
            animator.SetInteger(DirectionParameterHash, 0);
        }
        else if (Dir.y > 0)
        {
            animator.SetInteger(DirectionParameterHash, 2);
        }
        else if (Dir.x > 0)
        {
            animator.SetInteger(DirectionParameterHash, 1);
            spriteRenderer.flipX = false;
        }
        else if (Dir.x < 0)
        {
            animator.SetInteger(DirectionParameterHash, 1);
            spriteRenderer.flipX = true;
        }
    }
}
