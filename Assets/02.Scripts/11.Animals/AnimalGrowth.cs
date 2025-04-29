using UnityEngine;

public class AnimalGrowth : MonoBehaviour
{
    public GrowthManager growthManager;

    public int eatsToGrow = 3; // �������� �ٸ��� ���� ����
    private int eatCount = 0;

    public void OnEat()
    {
        eatCount++;
        if (eatCount >= eatsToGrow)
        {
            growthManager.GrowNextStage();
            eatCount = 0; // ���� �ܰ�� �غ�
        }
    }
}
