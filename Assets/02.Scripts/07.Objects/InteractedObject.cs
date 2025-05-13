using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConnectedObjectAndTime
{
    public int BeforeItemNum;
    public float Time;
    public int Amount;
    public int AfterItemNum;
}

public class InteractedObject : MonoBehaviour, IInteract
{
    public List<ConnectedObjectAndTime> COT;

    public virtual void Interact()
    {

    }

    protected ConnectedObjectAndTime FindObject(int num)
    {
        return COT.Find(ConnectedObjectAndTime => ConnectedObjectAndTime.BeforeItemNum == num);
    }
}
