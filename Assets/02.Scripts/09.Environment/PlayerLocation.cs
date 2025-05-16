using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public static PlayerLocation Instance;

    public bool IsIndoor { get; private set; }

    private int insideLayer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inside ���̾� �ε��� ��������
        insideLayer = LayerMask.NameToLayer("Inside");
        if (insideLayer == -1)
        {
            Debug.LogWarning("Layer 'Inside' �� �������� �ʽ��ϴ�. ���̾� ������ Ȯ���ϼ���.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Inside")
            && collision.gameObject.name == "IndoorAreaTrigger") // �̸� üũ
        {
            if (!IsIndoor)
            {
                IsIndoor = true;
                Weather.Instance?.HideWeatherEffect();
                Debug.Log("�ǳ� ����: ���� ȿ�� ��Ȱ��ȭ");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Inside")
            && collision.gameObject.name == "IndoorAreaTrigger") // �̸� üũ
        {
            if (IsIndoor)
            {
                IsIndoor = false;
                Weather.Instance?.ApplyWeather(Weather.Instance.CurrentWeather);
                Debug.Log("�ǿ� ����: ���� ȿ�� ����");
            }
        }
    }
}