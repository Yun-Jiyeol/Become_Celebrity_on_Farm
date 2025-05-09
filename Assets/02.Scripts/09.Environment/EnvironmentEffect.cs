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

    // ���ΰ��� Transform�� �߰�
    public Transform playerTransform;
    private Vector3 offset = new Vector3(0f, 2f, 0f); // �Ӹ� ��

    void Awake()
    {

        // ���ΰ��� Transform�� ã�Ƽ� �Ҵ�
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        Debug.Log("EnvironmentEffect ��ũ��Ʈ�� Awake()���� ����Ǿ����ϴ�.");
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
            effectParent.SetParent(playerTransform); // Player�� ����
            effectParent.localPosition = new Vector3(0f, 2f, 0f); // �Ӹ� ����
            Debug.Log("��ƼŬ �θ� Player�� �ٰ� ��ġ ������");
        }
        else
        {
            Debug.LogWarning("playerTransform �Ǵ� effectParent�� �������!");
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
                Debug.LogWarning($"[����Ʈ �ε� ����] {name} ��(��) Resources/Effects �������� ã�� �� �����ϴ�.");
            }
        }
        effects[name] = effectObj;
    }

    public void ApplyEffect(Weather.WeatherType weatherType)
    {
        Debug.Log($"[��ƼŬ ����] ���� ����: {weatherType}");

        if (!IsOutdoor())
        {
            // �ǳ�: ��� ��ƼŬ ����
            foreach (var kvp in effects)
                kvp.Value?.SetActive(false);

            Debug.Log("[�ǳ� ����] ���� ����Ʈ ��Ȱ��ȭ��");
            return;
        }

        // �ǿ�: ���� ���� ����, �ش� ����Ʈ�� ��
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

    // �ǳ�/�ǿ� ���°� ������� �� ����Ʈ �ٽ� ����
    public void RefreshEffect()
    {
        if (Weather.Instance != null)
        {
            ApplyEffect(Weather.Instance.CurrentWeather);
        }
    }

    
}