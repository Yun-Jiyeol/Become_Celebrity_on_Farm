using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGetWater : MonoBehaviour
{
    private void OnEnable()
    {
        MakeWater();
        TimeManager.Instance.OnDayChanged += Invokeit;
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnDayChanged -= Invokeit;
    }

    public void Invokeit()
    {
        Invoke("MakeWater", 2f);
    }

    void MakeWater()
    {
        if(gameObject.transform.tag == "Plow")
        {
            if (Weather.Instance.CurrentWeather == Weather.WeatherType.Rain)
            {
                ItemManager.Instance.spawnGround.SpawnGrounds(ChangedGround.Watered, gameObject.transform.position);
            }
        }
    }
}
