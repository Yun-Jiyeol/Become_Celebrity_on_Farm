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
//    [Header("����Ʈ �θ� �� ����Ʈ ������")]
//    public Transform effectParent;
//    public GameObject rainEffect;
//    public GameObject smallrainEffect;
//    public GameObject snowEffect;
//    public GameObject smallsnowEffect;
//    public GameObject flowerRainEffect;

//    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

//    [Header("�÷��̾�")]
//    public Transform playerTransform;
//    private Vector3 offset = new Vector3(0f, 2f, 0f); // �Ӹ� �� ����

//    // "Outside" ���̾� ��ȣ ����
//    private int outsideLayer;

//    void Awake()
//    {
//        // "Outside"��� ���̾� �̸��� �ش��ϴ� ��ȣ�� ������
//        outsideLayer = LayerMask.NameToLayer("Outside");

//        // �÷��̾� ã��
//        playerTransform = GameObject.FindWithTag("Player")?.transform;
//        if (playerTransform == null)
//            Debug.LogError("Player�� ã�� �� �����ϴ�. Tag�� 'Player'���� Ȯ���ϼ���.");

//        // ����Ʈ �ʱ�ȭ
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
//            Debug.LogWarning("effectParent �Ǵ� playerTransform�� ����ֽ��ϴ�.");
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

//        // �÷��̾ �ö� �ִ� Ÿ�ϸ��̳� ������Ʈ�� Layer�� �˻� (Layer�� "Outside"�� ��츸 true)
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
//                Debug.LogWarning($"����Ʈ '{name}'��(��) Resources/Effects/ ���� ã�� �� �����ϴ�.");
//            }
//        }

//        effects[name] = effectObj;
//    }

//    public void ApplyEffect(Weather.WeatherType weatherType)
//    {
//        Debug.Log($"[���� ����] ���� ����: {weatherType}");

//        // �ǿܰ� �ƴϸ� ���� ��
//        if (!IsOutdoor())
//        {
//            foreach (var kvp in effects)
//                kvp.Value?.SetActive(false);

//            Debug.Log("[�ǳ� ����] ���� ����Ʈ ��Ȱ��ȭ");
//            return;
//        }

//        // �ǿ��� ��: ��� ����Ʈ ���ٰ� �ش� ����Ʈ�� �ѱ�
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

//    // �ǳ�/�ǿܰ� ����Ǿ��ų�, ������ ����Ǿ��� �� ȣ��
//    public void RefreshEffect()
//    {
//        if (Weather.Instance != null)
//        {
//            ApplyEffect(Weather.Instance.CurrentWeather);
//        }
//    }
//}