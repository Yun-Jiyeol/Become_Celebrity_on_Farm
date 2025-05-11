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

    private void Update()
    {
        if (playerTransform != null)
        {
            // 플레이어 아래로 Raycast 발사
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, 1f);
            if (hit.collider != null)
            {
                // 부딪힌 오브젝트가 Outside Layer인지 확인
                IsIndoor = (outsideLayerMask.value & (1 << hit.collider.gameObject.layer)) == 0;
            }
        }
    }

}