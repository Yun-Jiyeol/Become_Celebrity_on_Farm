using UnityEngine;

public class GrowthManager : MonoBehaviour
{
    [System.Serializable]
    public class GrowthStage
    {
        public GameObject prefab;   // 해당 단계 프리팹
    }

    public GrowthStage[] stages; // Egg → BabyEgg → Chick → Chicken
    private int currentStageIndex = 0;

    private GameObject currentObject;

    void Start()
    {
        // 처음 시작 시 최초 단계 프리팹 생성
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