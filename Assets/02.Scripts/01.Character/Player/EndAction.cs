using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAction : MonoBehaviour
{
    public void EndingInteractionAction()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().EndAction();
    }
}
