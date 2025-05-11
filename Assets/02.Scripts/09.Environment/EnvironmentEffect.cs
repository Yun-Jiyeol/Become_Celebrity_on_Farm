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

    bool IsOutdoor()
    {
        return PlayerLocation.Instance != null && !PlayerLocation.Instance.IsIndoor;
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

    public void ApplyEffect(Weather.WeatherType weatherType)
    {
        if (!IsOutdoor())
        {
            foreach (var kvp in effects)
                kvp.Value?.SetActive(false);
            return;
        }

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

    public void RefreshEffect()
    {
        if (Weather.Instance != null)
            ApplyEffect(Weather.Instance.CurrentWeather);
    }
}//using UnityEngine;
 //using System.Collections.Generic;

//public class EnvironmentEffect : MonoBehaviour
//{
//    [Header("이펙트 부모 및 이펙트 프리팹")]
//    public Transform effectParent;
//    public GameObject rainEffect;
//    public GameObject smallrainEffect;
//    public GameObject snowEffect;
//    public GameObject smallsnowEffect;
//    public GameObject flowerRainEffect;

//    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

//    [Header("플레이어")]
//    public Transform playerTransform;
//    private Vector3 offset = new Vector3(0f, 2f, 0f); // 머리 위 고정

//    // "Outside" 레이어 번호 저장
//    private int outsideLayer;

//    void Awake()
//    {
//        // "Outside"라는 레이어 이름에 해당하는 번호를 가져옴
//        outsideLayer = LayerMask.NameToLayer("Outside");

//        // 플레이어 찾기
//        playerTransform = GameObject.FindWithTag("Player")?.transform;
//        if (playerTransform == null)
//            Debug.LogError("Player를 찾을 수 없습니다. Tag가 'Player'인지 확인하세요.");

//        // 이펙트 초기화
//        InitEffect("Rain", ref rainEffect);
//        InitEffect("SmallRain", ref smallrainEffect);
//        InitEffect("Snow", ref snowEffect);
//        InitEffect("SmallSnow", ref smallsnowEffect);
//        InitEffect("FlowerRain", ref flowerRainEffect);
//    }

//    void Start()
//    {
//        if (playerTransform != null && effectParent != null)
//        {
//            effectParent.SetParent(playerTransform);
//            effectParent.localPosition = offset;
//        }
//        else
//        {
//            Debug.LogWarning("effectParent 또는 playerTransform이 비어있습니다.");
//        }
//    }

//    void Update()
//    {
//        if (playerTransform != null && effectParent != null)
//        {
//            effectParent.position = playerTransform.position + offset;
//        }
//    }

//    bool IsOutdoor()
//    {
//        if (playerTransform == null) return false;

//        // 플레이어가 올라가 있는 타일맵이나 오브젝트의 Layer를 검사 (Layer가 "Outside"일 경우만 true)
//        return playerTransform.gameObject.layer == outsideLayer;
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
//                Debug.LogWarning($"이펙트 '{name}'을(를) Resources/Effects/ 에서 찾을 수 없습니다.");
//            }
//        }

//        effects[name] = effectObj;
//    }

//    public void ApplyEffect(Weather.WeatherType weatherType)
//    {
//        Debug.Log($"[날씨 적용] 현재 날씨: {weatherType}");

//        // 실외가 아니면 전부 끔
//        if (!IsOutdoor())
//        {
//            foreach (var kvp in effects)
//                kvp.Value?.SetActive(false);

//            Debug.Log("[실내 감지] 날씨 이펙트 비활성화");
//            return;
//        }

//        // 실외일 때: 모든 이펙트 껐다가 해당 이펙트만 켜기
//        foreach (var kvp in effects)
//            kvp.Value?.SetActive(false);

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

//    // 실내/실외가 변경되었거나, 날씨가 변경되었을 때 호출
//    public void RefreshEffect()
//    {
//        if (Weather.Instance != null)
//        {
//            ApplyEffect(Weather.Instance.CurrentWeather);
//        }
//    }
//}