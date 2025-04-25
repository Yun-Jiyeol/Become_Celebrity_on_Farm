using UnityEngine;
using System.Collections.Generic;

public class EnvironmentEffect : MonoBehaviour
{
    public Transform effectParent;

    public GameObject rainEffect;
    public GameObject smallrainEffect;
    public GameObject snowEffect;
    public GameObject smallsnowEffect;
    public GameObject flowerRainEffect;

    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    void Awake()
    {
        InitEffect("Rain", ref rainEffect);
        InitEffect("SmallRain", ref smallrainEffect);
        InitEffect("Snow", ref snowEffect);
        InitEffect("SmallSnow", ref smallsnowEffect);
        InitEffect("FlowerRain", ref flowerRainEffect);
    }

    void InitEffect(string name, ref GameObject effectObj)
    {
        if (effectObj == null)
        {
            GameObject loaded = Resources.Load<GameObject>($"Effects/{name}");
            if (loaded != null)
            {
                effectObj = Instantiate(loaded, effectParent);
                effectObj.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"[����Ʈ �ε� ����] {name} ��(��) Resources/Effects �������� ã�� �� �����ϴ�.");
            }
        }
        effects[name] = effectObj;
    }

    public void ApplyEffect(Weather.WeatherType weatherType)
    {
        Debug.Log($"[��ƼŬ ����] ���� ����: {weatherType}");

        foreach (var kvp in effects)
        {
            kvp.Value?.SetActive(false);
        }

        switch (weatherType)
        {
            case Weather.WeatherType.Rain:
                rainEffect?.SetActive(true);
                smallrainEffect?.SetActive(true);
                break;
            case Weather.WeatherType.Snow:
                snowEffect?.SetActive(true);
                smallsnowEffect?.SetActive(true);
                break;
            case Weather.WeatherType.FlowerRain:
                flowerRainEffect?.SetActive(true);
                break;
        }
    }
}