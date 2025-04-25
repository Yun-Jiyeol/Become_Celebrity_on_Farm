using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveDuration = 2f;
    public float idleDuration = 2f;

    private float timer;
    private Vector2 moveDir;
    private bool isMoving;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        ChooseNextState();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (isMoving)
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }

        if (timer <= 0f)
        {
            ChooseNextState();
        }

        // 애니메이션 파라미터 전달
        if (anim != null)
        {
            anim.SetBool("isWalking", isMoving);
            if (isMoving)
            {
                anim.SetFloat("moveX", moveDir.x);
                anim.SetFloat("moveY", moveDir.y);
            }
        }

        if (moveDir.x != 0 && GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().flipX = moveDir.x > 0;
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
}

