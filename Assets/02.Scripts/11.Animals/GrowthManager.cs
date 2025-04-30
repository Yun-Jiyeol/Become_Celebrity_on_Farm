using UnityEngine;

public class GrowthManager : MonoBehaviour
{
    [System.Serializable]
    public class GrowthStage
    {
        public GameObject prefab;   // �ش� �ܰ� ������
    }

    public GrowthStage[] stages; // Egg �� BabyEgg �� Chick �� Chicken
    private int currentStageIndex = 0;

    private GameObject currentObject;

    void Start()
    {
        // ó�� ���� �� ���� �ܰ� ������ ����
        currentObject = Instantiate(stages[currentStageIndex].prefab, transform.position, Quaternion.identity, transform);
    }

    public void GrowNextStage()
    {
        if (currentStageIndex < stages.Length - 1)
        {
            Destroy(currentObject);
            currentStageIndex++;
            currentObject = Instantiate(stages[currentStageIndex].prefab, transform.position, Quaternion.identity, transform);
        }
    }
}