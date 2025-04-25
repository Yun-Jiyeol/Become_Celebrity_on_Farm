using System.Collections;
using UnityEngine;

public class GrowthManager : MonoBehaviour
{
    [System.Serializable]
    public class GrowthStage
    {
        public GameObject prefab;   // 해당 단계 프리팹
        public float growTime;      // 다음 단계까지 걸리는 시간
    }

    public GrowthStage[] stages;      // Egg → BabyEgg → Chick → Chicken
    private int currentStageIndex = 0;

    private GameObject currentObject;

    void Start()
    {
        StartCoroutine(GrowRoutine());
    }

    IEnumerator GrowRoutine()
    {
        while (currentStageIndex < stages.Length)
        {
            // 현재 오브젝트 생성
            currentObject = Instantiate(stages[currentStageIndex].prefab, transform.position, Quaternion.identity, transform);

            // 마지막 스테이지면 루틴 종료 (삭제 X)
            if (currentStageIndex == stages.Length - 1)
                yield break;

            // 성장 시간 대기
            float timer = stages[currentStageIndex].growTime;
            yield return new WaitForSeconds(timer);

            // 다음 스테이지로 넘어가기 전 현재 오브젝트 제거
            if (currentObject != null)
                Destroy(currentObject);

            currentStageIndex++;
        }
    }
}
