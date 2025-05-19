using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    private bool isPlayerInRange = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log(" Q Ű ����");

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(" Q Ű ���� �� ����Ʈ ���� ����");
            OnInteractWithComputer();
        }
    }

    private void OnInteractWithComputer()
    {
        PlannerQuestManager.Instance.TryShowTodayQuest();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D: " + other.name);

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("�÷��̾� ���� �ȿ� ���� (isPlayerInRange = true)");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
        Debug.Log(" �÷��̾� �������� ���� (isPlayerInRange = false)");
    }
}