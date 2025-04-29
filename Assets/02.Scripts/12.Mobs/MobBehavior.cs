using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    public float attackPower = 10f;  // 공격력
    public float moveSpeed = 2f;     // 이동 속도

    private Transform player;        // 플레이어의 Transform
    private bool isPlayerInRange;    // 플레이어가 범위 내에 있는지 체크
    private Camera mainCamera;       // 메인 카메라

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;  // 플레이어를 찾음
        mainCamera = Camera.main;  // 메인 카메라 참조
    }

    void Update()
    {
        if (isPlayerInRange && IsPlayerVisible())
        {
            // 플레이어를 향해 이동
            MoveTowardsPlayer();

            // 플레이어와 근접하면 공격
            AttackPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // 플레이어를 향한 방향을 계산
            Vector3 direction = (player.position - transform.position).normalized;
            // 실제 이동
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void AttackPlayer()
    {
        // 공격 로직 구현 (간단히 공격력 표시)
        Debug.Log($"Attacking player with power: {attackPower}");
    }

    // 플레이어가 범위에 들어왔을 때 호출
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // 플레이어가 범위를 벗어났을 때 호출
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    // 카메라에 플레이어가 보이는지 체크하는 함수
    private bool IsPlayerVisible()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.position);

        // Viewport 좌표가 (0,0)에서 (1,1) 사이에 있으면 카메라에 보이는 위치
        if (viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1 && viewportPosition.z > 0)
        {
            return true;
        }
        return false;
    }
}