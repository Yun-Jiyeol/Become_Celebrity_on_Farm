using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveDuration = 2f;
    public float idleDuration = 2f;

    public GameObject heartEffectPrefab;

    private float timer;
    private Vector2 moveDir;
    private bool isMoving;

    private Animator anim;
    private Food targetFood;

    void Start()
    {
        anim = GetComponent<Animator>();
        ChooseNextState();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (targetFood == null)
            FindNearestFood();

        if (targetFood != null)
        {
            Vector2 dir = (targetFood.transform.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetFood.transform.position) < 0.3f)
            {
                EatFood();
            }
        }
        else
        {
            if (isMoving)
            {
                transform.Translate(moveDir * moveSpeed * Time.deltaTime);
            }
        }

        if (timer <= 0f)
        {
            ChooseNextState();
        }

        UpdateAnimation();
    }

    void FindNearestFood()
    {
        var foods = FindObjectsOfType<Food>();
        float minDist = Mathf.Infinity;
        foreach (var food in foods)
        {
            float dist = Vector2.Distance(transform.position, food.transform.position);
            if (dist < 5f && dist < minDist)
            {
                minDist = dist;
                targetFood = food;
            }
        }
    }

    void EatFood()
    {
        if (targetFood != null)
        {
            // 1. 먹이를 삭제
            Destroy(targetFood.gameObject);
            targetFood = null;

            // 2. 하트 이펙트 생성
            if (heartEffectPrefab != null)
            {
                GameObject heart = Instantiate(heartEffectPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity, transform);
                Destroy(heart, 1.0f); // 1초 뒤에 하트 제거
            }

            // 3. 성장 시도
            if (TryGetComponent<AnimalGrowth>(out var growth))
            {
                growth.OnEat();
                Debug.Log("성장!");
            }
        }
    }

    void ChooseNextState()
    {
        isMoving = Random.value > 0.5f;
        timer = isMoving ? moveDuration : idleDuration;

        if (isMoving)
        {
            moveDir = Random.insideUnitCircle.normalized;
        }
        else
        {
            moveDir = Vector2.zero;
        }
    }

    void UpdateAnimation()
    {
        if (anim != null)
        {
            anim.SetBool("isWalking", isMoving);
            if (isMoving)
            {
                anim.SetFloat("moveX", moveDir.x);
                anim.SetFloat("moveY", moveDir.y);
            }
        }
    }
}
