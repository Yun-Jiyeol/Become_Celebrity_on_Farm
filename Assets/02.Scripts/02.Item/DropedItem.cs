using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static ItemDataReader;
using static UnityEditor.Progress;

public class DropedItem : MonoBehaviour, IInteract
{
    ItemsData item;
    Vector3 startPosition;
    public int amount; //갯수

    public Collider2D collider2D;
    public Rigidbody2D rig;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public SortingOrderGroup sortingOrderGroup;

    Sprite _sprite;
    Coroutine coroutine;

    private void Start()
    {
        _sprite = spriteRenderer.sprite;
    }

    public void SpawnedDropItem(int _amunt, Vector3 _position, ItemsData itemData)
    {
        item = itemData;
        amount = _amunt;
        startPosition = _position;
        collider2D.enabled = false;
        transform.position = _position;
        transform.localScale = new Vector3(0.5f, 0.5f, 1);

        rig.gravityScale = 1;
        coroutine = StartCoroutine(DropItem());
    }

    IEnumerator DropItem()
    {
        float Angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle));
        Vector2 force = dir * 5f; //방향 설정 후 랜덤 힘만큼 쏜다

        rig.AddForce(force,ForceMode2D.Impulse); //힘을 준다

        while (Vector2.Distance(startPosition,transform.position) < 2f)
        {
            sortingOrderGroup.UpdateSortingOrderGroup();
            yield return null;
        }

        spriteRenderer.sprite = item.Item_sprite;
        animator.SetBool("Idle", true);
        rig.gravityScale = 0;
        rig.velocity = Vector2.zero;
        collider2D.enabled = true;

        while (transform.localScale.x <= 1f)
        {
            transform.localScale += new Vector3(1,1,0) * Time.deltaTime * 2f;
            yield return null;
        }
    }

    public void Interact()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        animator.SetBool("Idle", false);
        collider2D.enabled = false;

        coroutine = StartCoroutine(GetItem());
    }

    IEnumerator GetItem()
    {
        spriteRenderer.sprite = _sprite;

        while(transform.localScale.x >= 0.5f)
        {
            transform.localScale -= new Vector3(1, 1, 0) * Time.deltaTime * 5f;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        while(Vector2.Distance(GameManager.Instance.player.transform.position, transform.position) > 0.1f)
        {
            Vector3 dir = GameManager.Instance.player.transform.position - transform.position;
            transform.position += dir.normalized * 3f * Time.deltaTime;
            sortingOrderGroup.UpdateSortingOrderGroup();
            yield return null;
        }

        Destroy(gameObject);
        //인벤토리에 넣기
    }
}
