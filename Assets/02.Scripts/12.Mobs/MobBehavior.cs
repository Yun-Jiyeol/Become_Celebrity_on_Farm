using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    [Header("설정")]
    public float attackPower = 10f;           // 공격력
    public float moveSpeed = 2f;              // 이동 속도
    public float detectionRadius = 5f;        // 플레이어를 감지하는 반경

    private Transform player;
    private Camera mainCamera;

    private bool hasSeenPlayer = false;       // 플레이어를 본 적이 있는지 체크

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어를 본 적이 있거나, 감지 범위 내에 있을 경우
        if (hasSeenPlayer || (IsPlayerInRange() && IsPlayerVisible()))
        {
            MoveTowardsPlayer();
            AttackPlayer();
            AvoidOtherMobs(); // 겹침 방지
        }
    }

    bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRadius;
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        Debug.Log($"Attacking player with power: {attackPower}");
    }

    void AvoidOtherMobs()
    {
        GameObject[] allMobs = GameObject.FindGameObjectsWithTag("Mob");

        foreach (GameObject mob in allMobs)
        {
            if (mob != gameObject)
            {
                float distance = Vector2.Distance(transform.position, mob.transform.position);
                if (distance < 0.5f)
                {
                    Vector3 away = (transform.position - mob.transform.position).normalized;
                    transform.position += away * 0.01f;
                }
            }
        }
    }

    bool IsPlayerVisible()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.position);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z > 0;
    }

    // 플레이어를 감지하면 계속 따라오도록 상태를 변경
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasSeenPlayer = true; // 플레이어를 봤다고 설정
        }
    }
}