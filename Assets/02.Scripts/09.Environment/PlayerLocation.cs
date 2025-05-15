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

        // Inside 레이어 인덱스 가져오기
        insideLayer = LayerMask.NameToLayer("Inside");
        if (insideLayer == -1)
        {
            Debug.LogWarning("Layer 'Inside' 가 존재하지 않습니다. 레이어 설정을 확인하세요.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Inside")
            && collision.gameObject.name == "IndoorAreaTrigger") // 이름 체크
        {
            if (!IsIndoor)
            {
                IsIndoor = true;
                Weather.Instance?.HideWeatherEffect();
                Debug.Log("실내 진입: 날씨 효과 비활성화");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Inside")
            && collision.gameObject.name == "IndoorAreaTrigger") // 이름 체크
        {
            if (IsIndoor)
            {
                IsIndoor = false;
                Weather.Instance?.ApplyWeather(Weather.Instance.CurrentWeather);
                Debug.Log("실외 진입: 날씨 효과 적용");
            }
        }
    }
}