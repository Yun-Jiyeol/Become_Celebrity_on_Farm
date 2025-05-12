using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public static PlayerLocation Instance;

    public LayerMask outsideLayerMask;

    public bool IsIndoor { get; private set; }

    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        UpdateIndoorStateWithRaycast();
    }

    public void UpdateIndoorStateWithRaycast()
    {
        RaycastHit2D hitDown = Physics2D.Raycast(playerTransform.position, Vector2.down, 2f, outsideLayerMask);
        RaycastHit2D hitUp = Physics2D.Raycast(playerTransform.position, Vector2.up, 2f, outsideLayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(playerTransform.position, Vector2.left, 2f, outsideLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(playerTransform.position, Vector2.right, 2f, outsideLayerMask);

        bool isSurrounded = hitDown.collider == null &&
                            hitUp.collider == null &&
                            hitLeft.collider == null &&
                            hitRight.collider == null;

        bool isCurrentlyIndoor = isSurrounded;

        if (isCurrentlyIndoor != IsIndoor)
        {
            IsIndoor = isCurrentlyIndoor;

            if (Weather.Instance != null)
            {
                if (IsIndoor)
                {
                    Weather.Instance.HideWeatherEffect();
                    Debug.Log("실내 진입: 날씨 효과 비활성화");
                }
                else
                {
                    Weather.Instance.ApplyWeather(Weather.Instance.CurrentWeather);
                    Debug.Log("실외 진입: 날씨 효과 적용");
                }
            }
            else
            {
                Debug.LogWarning("Weather.Instance가 null입니다!");
            }
        }
    }
}
//if (playerTransform != null)
//{
//    //RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, 1f, outsideLayerMask);

//    RaycastHit2D hitDown = Physics2D.Raycast(playerTransform.position, Vector2.down, 2f, outsideLayerMask);
//    RaycastHit2D hitUp = Physics2D.Raycast(playerTransform.position, Vector2.up, 2f, outsideLayerMask);

//    // 둘 중 하나라도 맞으면 hit으로 간주
//    RaycastHit2D hit = hitDown.collider != null ? hitDown : hitUp;

//    Weather.Instance.ApplyWeather(Weather.Instance.CurrentWeather,!hit.collider);
//    Debug.Log(!hit.collider);

//}
