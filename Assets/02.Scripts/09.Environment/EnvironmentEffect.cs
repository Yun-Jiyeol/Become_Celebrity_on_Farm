using UnityEngine;

public class EnvironmentEffect : MonoBehaviour
{
    public GameObject rainEffect;
    public GameObject smallrainEffect;
    public GameObject snowEffect;
    public GameObject smallsnowEffect;
    public GameObject flowerRainEffect;

    public void ApplyEffect(Weather.WeatherType weatherType)
    {
        Debug.Log($"[파티클 적용] 현재 날씨: {weatherType}");

        rainEffect?.SetActive(false);
        smallrainEffect?.SetActive(false);
        snowEffect?.SetActive(false);
        smallsnowEffect?.SetActive(false);
        flowerRainEffect?.SetActive(false);

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