using UnityEngine;

public class AnimalGrowth : MonoBehaviour
{
    public GrowthManager growthManager;

    public int eatsToGrow = 3; // 동물마다 다르게 설정 가능
    private int eatCount = 0;

    public void OnEat()
    {
        eatCount++;
        if (eatCount >= eatsToGrow)
        {
            growthManager.GrowNextStage();
            eatCount = 0; // 다음 단계로 준비
        }
    }
}
