using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    [Header("���� ����")]
    public float maxHealth = 20f;
    [SerializeField] private float currentHealth; // �ν����Ϳ��� ���⸸ ����
    public float attackPower = 10f;
    public float moveSpeed = 2f;
    public float detectionRadius = 5f;

    [Header("���� ����")]
    [HideInInspector] public BoxCollider2D allowedArea;
    public float maxChaseDistance = 10f;

    [Header("�Ա� ���� ����")]
    public GameObject mineEntranceObject; // �ν����Ϳ��� ����

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
        // �Ա� ���� ��� ���� ��������
        if (mineEntranceObject != null && mineEntranceObject.activeInHierarchy)
        {
            Debug.Log($"{gameObject.name}: MineEntrance�� Ȱ��ȭ �����̹Ƿ� Mob ��Ȱ��ȭ��");
            gameObject.SetActive(false);
            return;
        }

        player = GameObject.FindWithTag("Player")?.transform;
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning($"{gameObject.name}: Main Camera�� ã�� ���߽��ϴ�.");
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
            Debug.LogWarning($"{gameObject.name}: 'IndoorArea' �±׸� ���� ������Ʈ�� ã�� ���߽��ϴ�.");
            return;
        }

        float minDist = Mathf.Infinity;

        foreach (GameObject area in areas)
        {
            if (area == null) continue;
            BoxCollider2D areaCollider = area.GetComponent<BoxCollider2D>();
            if (areaCollider == null)
            {
                Debug.LogWarning($"{area.name}: BoxCollider2D�� �����ϴ�.");
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
            Debug.LogWarning($"{gameObject.name}: BoxCollider2D�� �ִ� IndoorArea�� ã�� ���߽��ϴ�.");
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
            Debug.Log($"�÷��̾ ����: {attackPower}");
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