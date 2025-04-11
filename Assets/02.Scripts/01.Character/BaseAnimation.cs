using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimation : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected string DirectionParameterName = "Direction";
    protected string WalkParameterName = "Walk";

    public int DirectionParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }

    protected virtual void Initalize()
    {
        DirectionParameterHash = Animator.StringToHash(DirectionParameterName);
        WalkParameterHash = Animator.StringToHash(WalkParameterName);
    }
}
