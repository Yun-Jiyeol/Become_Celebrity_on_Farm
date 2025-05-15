using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("»óÁ¡ µµÂø!");
            PlannerQuestManager.Instance.ReportAction("VisitShop");
        }
    }
}