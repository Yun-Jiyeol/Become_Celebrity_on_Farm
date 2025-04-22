using UnityEngine;
public class EnvironmentEffect : MonoBehaviour
{
    public GameObject rainEffect;
    public GameObject smallrainEffect;
    public GameObject snowEffect;
    public GameObject smallsnowEffect;
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
        Debug.Log($"현재 날씨: {weatherType}");

        // 모든 날씨 효과 비활성화
        rainEffect.SetActive(false);
        smallrainEffect.SetActive(false);
        snowEffect.SetActive(false);
        smallsnowEffect.SetActive(false);
        flowerRainEffect.SetActive(false);

        switch (weatherType)
        {
            case Weather.WeatherType.Rain:
                rainEffect.SetActive(true);
                smallrainEffect.SetActive(true);
                break;
            case Weather.WeatherType.Snow:
                snowEffect.SetActive(true);
                smallsnowEffect.SetActive(true);
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
