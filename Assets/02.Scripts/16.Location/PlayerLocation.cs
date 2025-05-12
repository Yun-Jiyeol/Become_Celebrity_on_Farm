using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public static PlayerLocation Instance;

    public LayerMask outsideLayerMask;  // Outside Layer만 포함

    public bool IsIndoor { get; private set; }

    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    //private void Update()
    //{
    //    UpdateIndoorStateWithRaycast();

    //    // 디버그용 Ray 시각화
    //    Vector2 origin = transform.position;
    //    Vector2 direction = Vector2.down;
    //    float distance = 1f;

    //    Debug.DrawRay(origin, direction * distance, Color.red);
    //}

    public void UpdateIndoorStateWithRaycast()
    {
        if (playerTransform != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, 1f, outsideLayerMask);
            Weather.Instance.ApplyWeather(Weather.Instance.CurrentWeather,!hit.collider);
            Debug.Log(!hit.collider);

        }
    }
}