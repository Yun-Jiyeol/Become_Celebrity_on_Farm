using UnityEngine;
using System.Collections.Generic;
using static Weather;

public class EnvironmentEffect : MonoBehaviour
{
    public Transform effectParent;

    public GameObject rainEffect;
    public GameObject smallrainEffect;
    public GameObject snowEffect;
    public GameObject smallsnowEffect;
    public GameObject flowerRainEffect;

    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    public Transform playerTransform;
    private Vector3 offset = new Vector3(0f, 2f, 0f);

    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        InitEffect("Rain", ref rainEffect);
        InitEffect("SmallRain", ref smallrainEffect);
        InitEffect("Snow", ref snowEffect);
        InitEffect("SmallSnow", ref smallsnowEffect);
        InitEffect("FlowerRain", ref flowerRainEffect);
    }

    void Start()
    {
        if (playerTransform != null && effectParent != null)
        {
            effectParent.SetParent(playerTransform);
            effectParent.localPosition = offset;
        }
    }

    void Update()
    {
        if (playerTransform != null && effectParent != null)
        {
            effectParent.position = playerTransform.position + offset;
        }
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
        }
        effects[name] = effectObj;
    }

    public void ApplyEffect(Weather.WeatherType weatherType, bool IsOutside)
    {
        // 실내일 경우 모든 파티클을 끄고 리턴
        if (!IsOutside)
        {
            foreach (var kvp in effects)
                kvp.Value?.SetActive(false);
            return;
        }

        // 모든 파티클을 끄고, 새로운 날씨에 맞는 파티클만 활성화
        foreach (var kvp in effects)
            kvp.Value?.SetActive(false);

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

    //public void RefreshEffect()
    //{
    //    if (Weather.Instance != null)
    //    {
    //        ApplyEffect(Weather.Instance.CurrentWeather);
    //    }
    //}
}