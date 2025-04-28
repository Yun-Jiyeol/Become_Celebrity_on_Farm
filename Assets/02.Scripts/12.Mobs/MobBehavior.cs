using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    public float attackPower = 10f; // ���ݷ�
    public float moveSpeed = 2f;    // �̵� �ӵ�

    private Transform player;       // �÷��̾��� Transform
    private bool isPlayerInRange;   // �÷��̾ ���� ���� �ִ��� üũ

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;  // �÷��̾ ã��
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            // �÷��̾ ���� �̵�
            MoveTowardsPlayer();

            // �÷��̾�� �����ϸ� ����
            AttackPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void AttackPlayer()
    {
        // ���� ���� ���� (������ ���ݷ� ǥ��)
        Debug.Log($"Attacking player with power: {attackPower}");
    }

    // �÷��̾ ������ ������ �� ȣ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // �÷��̾ ������ ����� �� ȣ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}