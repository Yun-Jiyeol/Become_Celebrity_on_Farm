using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    [Header("����")]
    public float maxHealth = 20f;
    private float currentHealth;
    public float attackPower = 10f;           // ���ݷ�
    public float moveSpeed = 2f;              // �̵� �ӵ�
    public float detectionRadius = 5f;        // �÷��̾ �����ϴ� �ݰ�

    private Transform player;
    private Camera mainCamera;

    private bool hasSeenPlayer = false;       // �÷��̾ �� ���� �ִ��� üũ

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        mainCamera = Camera.main;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null) return;

        // �÷��̾ �� ���� �ְų�, ���� ���� ���� ���� ���
        if (hasSeenPlayer || (IsPlayerInRange() && IsPlayerVisible()))
        {
            MoveTowardsPlayer();
            AttackPlayer();
            AvoidOtherMobs(); // ��ħ ����
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
        Debug.Log($"�÷��̾ ����: {attackPower}");
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
        if(currentHealth<=0)
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
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.position);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z > 0;
    }

    // �÷��̾ �����ϸ� ��� ��������� ���¸� ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasSeenPlayer = true; // �÷��̾ �ôٰ� ����
        }
    }
}