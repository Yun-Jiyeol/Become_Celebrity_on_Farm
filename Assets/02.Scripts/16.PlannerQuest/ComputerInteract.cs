using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange)
        {
            Debug.Log("플레이어 범위 안에 있음");
            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("C 키 눌림 → 퀘스트 열기");
                OnInteractWithComputer();
            }
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
            Debug.Log("플레이어 범위 안으로 들어옴");
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }
}