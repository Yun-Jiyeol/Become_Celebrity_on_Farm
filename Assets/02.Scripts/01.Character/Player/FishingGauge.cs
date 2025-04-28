using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingGauge : MonoBehaviour
{
    public GameObject ShowedGauge;

    public void MoveGauge(float Percentage)
    {
        ShowedGauge.transform.localPosition = new Vector3(1.5f , 2f * Percentage, 0);
    }
}
