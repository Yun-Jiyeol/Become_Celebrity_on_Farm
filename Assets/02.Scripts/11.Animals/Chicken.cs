using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ChickenStage
{
    Egg,
    Chick,
    Chicken
}

public class Chicken : MonoBehaviour
{
    public ChickenStage currentStage = ChickenStage.Egg;
    public SpriteRenderer spriteRenderer;
    public Sprite[] eggSprites;     // 알 상태 스프라이트 (애니메이션용 or 정지)
    public Sprite[] chickSprites;   // 병아리 스프라이트
    public Sprite[] chickenSprites; // 닭 스프라이트

    public float growTimeEgg = 5f;
    public float growTimeChick = 10f;
    private float growTimer;

    void Start()
    {
        SetStage(currentStage);
    }

    void Update()
    {
        growTimer += Time.deltaTime;

        if (currentStage == ChickenStage.Egg && growTimer >= growTimeEgg)
        {
            SetStage(ChickenStage.Chick);
        }
        else if (currentStage == ChickenStage.Chick && growTimer >= growTimeChick)
        {
            SetStage(ChickenStage.Chicken);
        }
    }

    void SetStage(ChickenStage newStage)
    {
        currentStage = newStage;
        growTimer = 0f;

        switch (newStage)
        {
            case ChickenStage.Egg:
                spriteRenderer.sprite = eggSprites[0];
                break;
            case ChickenStage.Chick:
                spriteRenderer.sprite = chickSprites[0];
                break;
            case ChickenStage.Chicken:
                spriteRenderer.sprite = chickenSprites[0];
                break;
        }
    }
}

