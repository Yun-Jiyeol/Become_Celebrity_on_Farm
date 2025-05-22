using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    [Header("스탯 설정")]
    public float maxHealth = 20f;
    [SerializeField] private float currentHealth; // 인스펙터에서 보기만 가능
    public float attackPower = 10f;
    public float moveSpeed = 2f;
    public float detectionRadius = 5f;

    [Header("영역 제한")]
    [HideInInspector] public BoxCollider2D allowedArea;
    public float maxChaseDistance = 10f;

    [Header("입구 상태 감지")]
    public GameObject mineEntranceObject; // 인스펙터에서 설정

    private Vector3 spawnPoint;
    private Transform player;
    private Camera mainCamera;

    private bool hasSeenPlayer = false;

    void OnEnable()
    {
        TryAssignIndoorArea();
    }

    void Start()
    {
        // 입구 씬일 경우 몬스터 꺼버리기
        if (mineEntranceObject != null && mineEntranceObject.activeInHierarchy)
        {
            Debug.Log($"{gameObject.name}: MineEntrance가 활성화 상태이므로 Mob 비활성화됨");
            gameObject.SetActive(false);
            return;
        }

        player = GameObject.FindWithTag("Player")?.transform;
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning($"{gameObject.name}: Main Camera를 찾지 못했습니다.");
        }

        currentHealth = maxHealth;
        spawnPoint = transform.position;

        TryAssignIndoorArea();
    }

    void Update()
    {
        if (player == null) return;

        if (hasSeenPlayer || (IsPlayerInRange() && IsPlayerVisible()))
        {
            if (Vector3.Distance(spawnPoint, player.position) <= maxChaseDistance)
            {
                MoveTowardsPlayer();
                AttackPlayer();
                AvoidOtherMobs();
            }
        }
    }

    void TryAssignIndoorArea()
    {
        if (allowedArea != null) return;

        GameObject[] areas = GameObject.FindGameObjectsWithTag("IndoorArea");
        if (areas == null || areas.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name}: 'IndoorArea' 태그를 가진 오브젝트를 찾지 못했습니다.");
            return;
        }

        float minDist = Mathf.Infinity;

        foreach (GameObject area in areas)
        {
            if (area == null) continue;
            BoxCollider2D areaCollider = area.GetComponent<BoxCollider2D>();
            if (areaCollider == null)
            {
                Debug.LogWarning($"{area.name}: BoxCollider2D가 없습니다.");
                continue;
            }

            float dist = Vector3.Distance(transform.position, areaCollider.bounds.center);
            if (dist < minDist)
            {
                minDist = dist;
                allowedArea = areaCollider;
            }
        }

        if (allowedArea == null)
        {
            Debug.LogWarning($"{gameObject.name}: BoxCollider2D가 있는 IndoorArea를 찾지 못했습니다.");
        }
    }

    bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRadius;
    }

    void MoveTowardsPlayer()
    {
        Vector3 targetPos = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (allowedArea == null || allowedArea.bounds.Contains(targetPos))
        {
            transform.position = targetPos;
        }
    }

    void AttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= 1f)
        {
            Debug.Log($"플레이어를 공격: {attackPower}");
        }
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

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    bool IsPlayerVisible()
    {
        if (mainCamera == null) return false;

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.position);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z > 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasSeenPlayer = true;
        }
    }
}