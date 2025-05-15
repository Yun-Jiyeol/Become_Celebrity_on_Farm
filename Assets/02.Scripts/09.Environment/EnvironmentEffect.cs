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

    private Vector3 offset = new Vector3(0f, 2f, 0f); // 카메라 기준 오프셋

    void Awake()
    {
        InitEffect("Rain", ref rainEffect);
        InitEffect("SmallRain", ref smallrainEffect);
        InitEffect("Snow", ref snowEffect);
        InitEffect("SmallSnow", ref smallsnowEffect);
        InitEffect("FlowerRain", ref flowerRainEffect);
    }

    void Start()
    {
        if (effectParent != null && Camera.main != null)
        {
            effectParent.SetParent(Camera.main.transform);
            effectParent.localPosition = offset;
        }
    }

    void Update()
    {
        if (effectParent != null && Camera.main != null)
        {
            effectParent.position = Camera.main.transform.position + offset;
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

    /// <summary>
    /// 날씨 파티클 적용
    /// </summary>
    /// <param name="weatherType">현재 날씨</param>
    /// <param name="IsOutside">실외 여부 (실내면 파티클 비활성화)</param>
    public void ApplyEffect(WeatherType weatherType, bool IsOutside)
    {
        // 실내일 경우 모든 파티클을 끄고 리턴
        if (!IsOutside)
        {
            foreach (var kvp in effects)
            {
                if (kvp.Value != null && kvp.Value.activeSelf)
                {
                    kvp.Value.SetActive(false);
                }
            }
            return;
        }

        // 실외일 경우 모든 파티클 끄기 (중복 활성화 방지)
        foreach (var kvp in effects)
        {
            if (kvp.Value != null && kvp.Value.activeSelf)
            {
                kvp.Value.SetActive(false);
            }
        }

        // 날씨별 파티클 활성화
        switch (weatherType)
        {
            case WeatherType.Rain:
                rainEffect?.SetActive(true);
                smallrainEffect?.SetActive(true);
                break;
            case WeatherType.Snow:
                snowEffect?.SetActive(true);
                smallsnowEffect?.SetActive(true);
                break;
            case WeatherType.FlowerRain:
                flowerRainEffect?.SetActive(true);
                break;
            case WeatherType.Sunny:
            default:
                // 맑음은 파티클 모두 꺼짐 상태 유지
                break;
        }
    }
}


//using UnityEngine;
//using System.Collections.Generic;

//public class EnvironmentEffect : MonoBehaviour
//{
//    public Transform effectParent;

//    public GameObject rainEffect;
//    public GameObject smallrainEffect;
//    public GameObject snowEffect;
//    public GameObject smallsnowEffect;
//    public GameObject flowerRainEffect;

//    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

//    void Awake()
//    {
//        InitEffect("Rain", ref rainEffect);
//        InitEffect("SmallRain", ref smallrainEffect);
//        InitEffect("Snow", ref snowEffect);
//        InitEffect("SmallSnow", ref smallsnowEffect);
//        InitEffect("FlowerRain", ref flowerRainEffect);
//    }

//    void InitEffect(string name, ref GameObject effectObj)
//    {
//        if (effectObj == null)
//        {
//            GameObject loaded = Resources.Load<GameObject>($"Effects/{name}");
//            if (loaded != null)
//            {
//                effectObj = Instantiate(loaded, effectParent);
//                effectObj.SetActive(false);
//            }
//            else
//            {
//                Debug.LogWarning($"[이펙트 로딩 실패] {name} 을(를) Resources/Effects 폴더에서 찾을 수 없습니다.");
//            }
//        }
//        effects[name] = effectObj;
//    }

//    public void ApplyEffect(Weather.WeatherType weatherType)
//    {
//        Debug.Log($"[파티클 적용] 현재 날씨: {weatherType}");

//        foreach (var kvp in effects)
//        {
//            kvp.Value?.SetActive(false);
//        }

//        switch (weatherType)
//        {
//            case Weather.WeatherType.Rain:
//                rainEffect?.SetActive(true);
//                smallrainEffect?.SetActive(true);
//                break;
//            case Weather.WeatherType.Snow:
//                snowEffect?.SetActive(true);
//                smallsnowEffect?.SetActive(true);
//                break;
//            case Weather.WeatherType.FlowerRain:
//                flowerRainEffect?.SetActive(true);
//                break;
//        }
//    }
//}