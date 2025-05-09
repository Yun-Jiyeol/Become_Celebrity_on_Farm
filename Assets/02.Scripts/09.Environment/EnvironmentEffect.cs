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

    // 주인공의 Transform을 추가
    public Transform playerTransform;
    private Vector3 offset = new Vector3(0f, 2f, 0f); // 머리 위

    void Awake()
    {

        // 주인공의 Transform을 찾아서 할당
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        Debug.Log("EnvironmentEffect 스크립트가 Awake()에서 실행되었습니다.");
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
            effectParent.SetParent(playerTransform); // Player에 붙임
            effectParent.localPosition = new Vector3(0f, 2f, 0f); // 머리 위로
            Debug.Log("파티클 부모가 Player에 붙고 위치 조정됨");
        }
        else
        {
            Debug.LogWarning("playerTransform 또는 effectParent가 비어있음!");
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
            else
            {
                Debug.LogWarning($"[이펙트 로딩 실패] {name} 을(를) Resources/Effects 폴더에서 찾을 수 없습니다.");
            }
        }
        effects[name] = effectObj;
    }

    public void ApplyEffect(Weather.WeatherType weatherType)
    {
        Debug.Log($"[파티클 적용] 현재 날씨: {weatherType}");

        if (!IsOutdoor())
        {
            // 실내: 모든 파티클 끄기
            foreach (var kvp in effects)
                kvp.Value?.SetActive(false);

            Debug.Log("[실내 상태] 날씨 이펙트 비활성화됨");
            return;
        }

        // 실외: 먼저 전부 끄고, 해당 이펙트만 켬
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

    // 실내/실외 상태가 변경됐을 때 이펙트 다시 적용
    public void RefreshEffect()
    {
        if (Weather.Instance != null)
        {
            ApplyEffect(Weather.Instance.CurrentWeather);
        }
    }

    
}