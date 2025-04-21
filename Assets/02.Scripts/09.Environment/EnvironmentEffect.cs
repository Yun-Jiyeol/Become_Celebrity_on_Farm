using UnityEngine;

public class EnvironmentEffect : MonoBehaviour
{
    public GameObject rainEffect;
    public GameObject snowEffect;
    public GameObject flowerRainEffect;

    private Weather weather;

    void Start()
    {
        weather = FindObjectOfType<Weather>();

        if (weather != null)
        {
            Weather.WeatherType todayWeather = weather.GetWeather(weather.currentDay);
            ApplyEffect(todayWeather);
        }
    }

    public void ApplyEffect(Weather.WeatherType weatherType)
    {
        rainEffect.SetActive(false);
        snowEffect.SetActive(false);
        flowerRainEffect.SetActive(false);

        switch (weatherType)
        {
            case Weather.WeatherType.Rain:
                rainEffect.SetActive(true);
                break;
            case Weather.WeatherType.Snow:
                snowEffect.SetActive(true);
                break;
            case Weather.WeatherType.FlowerRain:
                flowerRainEffect.SetActive(true);
                break;
            default:
                // Sunny - 아무것도 출력 안 함
                break;
        }
    }
}