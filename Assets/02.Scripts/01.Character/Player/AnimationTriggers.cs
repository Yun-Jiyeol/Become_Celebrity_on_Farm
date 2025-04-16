using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    public void SpawnAction()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().SpawnObject();
    }

    public void RangeInteractionAction()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().RangeInteractObject();
    }

    public void EndingInteractionAction()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().EndAction();
    }
}
