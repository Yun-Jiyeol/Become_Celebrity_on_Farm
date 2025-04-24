using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    string SpeedParameterName = "Speed";
    string HoeParameterName = "Hoe";
    string WateringParameterName = "Watering";
    string SickleParameterName = "Sickle";
    string PickaxeParameterName = "PickAxe";
    string AxeParameterName = "Axe";

    public int SpeedParameterHash { get; private set; }
    public int HoeParameterHash { get; private set; }
    public int WateringParameterHash { get; private set; }
    public int SickleParameterHash { get; private set; }
    public int PickaxeParameterHash { get; private set; }
    public int AxeParameterHash { get; private set; }

    private void Start()
    {
        animator = gameObject.GetComponent<Player>().animator;
        spriteRenderer = gameObject.GetComponent<Player>().spriteRenderer;

        Initalize();
    }

    protected override void Initalize()
    {
        base.Initalize();

        SpeedParameterHash = Animator.StringToHash(SpeedParameterName);
        HoeParameterHash = Animator.StringToHash(HoeParameterName);
        WateringParameterHash = Animator.StringToHash(WateringParameterName);
        SickleParameterHash = Animator.StringToHash(SickleParameterName);
        PickaxeParameterHash = Animator.StringToHash(PickaxeParameterName);
        AxeParameterHash = Animator.StringToHash(AxeParameterName);
    }

    private void FixedUpdate()
    {
        Dir = gameObject.GetComponent<Player>().playerController.dir;
    }

    protected override void Update()
    {
        base.Update();
    }
}
