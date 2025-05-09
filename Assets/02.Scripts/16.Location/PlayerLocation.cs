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
        SetIndoorState(true);  // ���� �� �ǳ��� ����
    }

    public void SetIndoorState(bool isIndoor)
    {
        IsIndoor = isIndoor;
        Debug.Log("�÷��̾� ���� ��ġ: " + (isIndoor ? "�ǳ�" : "�ǿ�"));

        // ���� ����Ʈ ����
        if (EnvironmentEffectInstanceExists())
        {
            FindObjectOfType<EnvironmentEffect>().RefreshEffect();
        }
        else
        {
            Debug.LogWarning("[PlayerLocation] EnvironmentEffect �ν��Ͻ��� ã�� �� �����ϴ�.");
        }
    }

    private bool EnvironmentEffectInstanceExists()
    {
        return FindObjectOfType<EnvironmentEffect>() != null;
    }
}