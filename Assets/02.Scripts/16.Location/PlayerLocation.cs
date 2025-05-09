using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public static PlayerLocation Instance { get; private set; }

    public bool IsIndoor { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        SetIndoorState(true);  // 시작 시 실내로 설정
    }

    public void SetIndoorState(bool isIndoor)
    {
        IsIndoor = isIndoor;
        Debug.Log("플레이어 현재 위치: " + (isIndoor ? "실내" : "실외"));

        // 날씨 이펙트 갱신
        if (EnvironmentEffectInstanceExists())
        {
            FindObjectOfType<EnvironmentEffect>().RefreshEffect();
        }
        else
        {
            Debug.LogWarning("[PlayerLocation] EnvironmentEffect 인스턴스를 찾을 수 없습니다.");
        }
    }

    private bool EnvironmentEffectInstanceExists()
    {
        return FindObjectOfType<EnvironmentEffect>() != null;
    }
}