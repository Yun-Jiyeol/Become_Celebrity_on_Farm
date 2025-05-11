using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public static PlayerLocation Instance;

    public LayerMask outsideLayerMask;  // Outside Layer�� ����

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
            // �÷��̾� �Ʒ��� Raycast �߻�
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, 1f);
            if (hit.collider != null)
            {
                // �ε��� ������Ʈ�� Outside Layer���� Ȯ��
                IsIndoor = (outsideLayerMask.value & (1 << hit.collider.gameObject.layer)) == 0;
            }
        }
    }

}