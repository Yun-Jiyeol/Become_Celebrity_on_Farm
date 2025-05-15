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

    private Vector3 offset = new Vector3(0f, 2f, 0f); // ī�޶� ���� ������

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
    /// ���� ��ƼŬ ����
    /// </summary>
    /// <param name="weatherType">���� ����</param>
    /// <param name="IsOutside">�ǿ� ���� (�ǳ��� ��ƼŬ ��Ȱ��ȭ)</param>
    public void ApplyEffect(WeatherType weatherType, bool IsOutside)
    {
        // �ǳ��� ��� ��� ��ƼŬ�� ���� ����
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

        // �ǿ��� ��� ��� ��ƼŬ ���� (�ߺ� Ȱ��ȭ ����)
        foreach (var kvp in effects)
        {
            if (kvp.Value != null && kvp.Value.activeSelf)
            {
                kvp.Value.SetActive(false);
            }
        }

        // ������ ��ƼŬ Ȱ��ȭ
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
                // ������ ��ƼŬ ��� ���� ���� ����
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
//                Debug.LogWarning($"[����Ʈ �ε� ����] {name} ��(��) Resources/Effects �������� ã�� �� �����ϴ�.");
//            }
//        }
//        effects[name] = effectObj;
//    }

//    public void ApplyEffect(Weather.WeatherType weatherType)
//    {
//        Debug.Log($"[��ƼŬ ����] ���� ����: {weatherType}");

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