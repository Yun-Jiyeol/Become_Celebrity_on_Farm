using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    void Start() 
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.simulated = true;

        ChooseNextState();
    }

    void FixedUpdate()
    {
        if (targetFood == null)
            FindNearestFood();

        Vector2 moveVector = Vector2.zero;

        if (targetFood != null)
        {
            Vector2 dir = (targetFood.transform.position - transform.position).normalized;
            moveVector = dir;

            if (Vector2.Distance(transform.position, targetFood.transform.position) < 0.3f)
            {
                EatFood();
            }
        }
        else if (isMoving)
        {
            moveVector = moveDir;
        }

        rb.MovePosition(rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
        UpdateAnimation(moveVector);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
            ChooseNextState();
    

        if (rb == null)
        {
            Debug.LogWarning($"[AnimalMovement] {gameObject.name}에 Rigidbody2D가 없습니다.");
            return;
        }
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

    void UpdateAnimation(Vector2 dir)
    {
        if (anim != null)
        {
            anim.SetBool("isWalking", dir != Vector2.zero);
            anim.SetFloat("moveX", dir.x);
            anim.SetFloat("moveY", dir.y);

            if (sprite != null)
                sprite.flipX = dir.x > 0;
        }
    }


}
