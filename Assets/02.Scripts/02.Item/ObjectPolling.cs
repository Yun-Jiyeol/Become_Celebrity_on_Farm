using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPolling : MonoBehaviour
{
    protected List<GameObject> Things = new List<GameObject>();

    protected virtual GameObject SpawnOrFindThings()
    {
        GameObject go = FindOffthings();
        return go;
    }

    protected GameObject FindOffthings()
    {
        if (Things.Count == 0) return null;

        foreach (GameObject thing in Things)
        {
            if (!thing.activeSelf)
            {
                return thing;
            }
        }
        return null;
    }

    public void OffAllThings()
    {
        foreach (GameObject thing in Things)
        {
            if (thing.activeSelf)
            {
                //≤®¡÷±‚
                thing.GetComponent<DropedItem>().offObject();
            }
        }
    }
}
