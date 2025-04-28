using System.Collections;
using UnityEngine;

public class GrowableAnimal : MonoBehaviour
{
    public enum GrowthStage { Egg, BabyEgg, Chick, Chicken }

    public GrowthStage currentStage = GrowthStage.Egg;

    public GameObject babyEggPrefab;
    public GameObject chickPrefab;
    public GameObject chickenPrefab;

    public float timeToBabyEgg = 5f;
    public float timeToChick = 10f;
    public float timeToChicken = 15f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        switch (currentStage)
        {
            case GrowthStage.Egg:
                if (timer >= timeToBabyEgg)
                    GrowToNextStage(babyEggPrefab, GrowthStage.BabyEgg);
                break;
            case GrowthStage.BabyEgg:
                if (timer >= timeToChick)
                    GrowToNextStage(chickPrefab, GrowthStage.Chick);
                break;
            case GrowthStage.Chick:
                if (timer >= timeToChicken)
                    GrowToNextStage(chickenPrefab, GrowthStage.Chicken);
                break;
        }
    }

    void GrowToNextStage(GameObject nextPrefab, GrowthStage nextStage)
    {
        var obj = Instantiate(nextPrefab, transform.position, Quaternion.identity);
        obj.name = nextStage.ToString();
        Destroy(gameObject);
    }
}

