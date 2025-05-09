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
        // 주인공이 있는지 체크하고, 주인공이 있다면 파티클들을 주인공을 따라가게 함
        if (playerTransform != null)
        {
            // 파티클 효과들이 활성화되어 있다면 주인공을 따라가게 위치를 업데이트
            foreach (var kvp in effects)
            {
                if (kvp.Value.activeSelf)  // 이펙트가 활성화되어 있는 경우
                {
                    Vector3 pos = playerTransform.position;
                    pos.y += 2f; // 주인공 머리 위로 약간 띄움
                    effectParent.position = pos;

                }
            }
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

        // 실내면 이펙트 모두 끄고 return
        if (PlayerLocation.Instance != null && PlayerLocation.Instance.IsIndoor)
        {
            foreach (var kvp in effects)
                kvp.Value?.SetActive(false);

            Debug.Log("[실내 상태] 날씨 이펙트 비활성화됨");
            return;
        }

        // 실외: 날씨에 맞는 이펙트만 켬
        foreach (var kvp in effects)
            kvp.Value?.SetActive(false);

        switch (weatherType)
        {
            case Weather.WeatherType.Rain:
                rainEffect?.SetActive(true);
                smallrainEffect?.SetActive(true);
                Debug.Log("비 이펙트 활성화");
                break;
            case Weather.WeatherType.Snow:
                snowEffect?.SetActive(true);
                smallsnowEffect?.SetActive(true);
                Debug.Log("눈 이펙트 활성화");
                break;
            case Weather.WeatherType.FlowerRain:
                flowerRainEffect?.SetActive(true);
                Debug.Log("꽃비 이펙트 활성화");
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