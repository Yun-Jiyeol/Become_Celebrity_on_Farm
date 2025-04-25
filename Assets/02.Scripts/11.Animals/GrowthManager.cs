using System.Collections;
using UnityEngine;

public class GrowthManager : MonoBehaviour
{
    [System.Serializable]
    public class GrowthStage
    {
        public GameObject prefab;   // �ش� �ܰ� ������
        public float growTime;      // ���� �ܰ���� �ɸ��� �ð�
    }

    public GrowthStage[] stages;      // Egg �� BabyEgg �� Chick �� Chicken
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
            // ���� ������Ʈ ����
            currentObject = Instantiate(stages[currentStageIndex].prefab, transform.position, Quaternion.identity, transform);

            // ������ ���������� ��ƾ ���� (���� X)
            if (currentStageIndex == stages.Length - 1)
                yield break;

            // ���� �ð� ���
            float timer = stages[currentStageIndex].growTime;
            yield return new WaitForSeconds(timer);

            // ���� ���������� �Ѿ�� �� ���� ������Ʈ ����
            if (currentObject != null)
                Destroy(currentObject);

            currentStageIndex++;
        }
    }
}
