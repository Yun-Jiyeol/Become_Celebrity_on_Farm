using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    public float attackPower = 10f;  // ���ݷ�
    public float moveSpeed = 2f;     // �̵� �ӵ�

    private Transform player;        // �÷��̾��� Transform
    private bool isPlayerInRange;    // �÷��̾ ���� ���� �ִ��� üũ
    private Camera mainCamera;       // ���� ī�޶�

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;  // �÷��̾ ã��
        mainCamera = Camera.main;  // ���� ī�޶� ����
    }

    void Update()
    {
        if (isPlayerInRange && IsPlayerVisible())
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
            // �÷��̾ ���� ������ ���
            Vector3 direction = (player.position - transform.position).normalized;
            // ���� �̵�
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

    // ī�޶� �÷��̾ ���̴��� üũ�ϴ� �Լ�
    private bool IsPlayerVisible()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.position);

        // Viewport ��ǥ�� (0,0)���� (1,1) ���̿� ������ ī�޶� ���̴� ��ġ
        if (viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1 && viewportPosition.z > 0)
        {
            return true;
        }
        return false;
    }
}