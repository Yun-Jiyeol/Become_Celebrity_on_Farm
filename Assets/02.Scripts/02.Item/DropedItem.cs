using System;
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
    public int amount; //°¹¼ö

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

    public void SpawnedDropItem(int _amount, Vector3 _position, ItemsData itemData)
    {
        item = itemData;
        amount = _amount;
        startPosition = _position;
        gameObject.tag = "Untagged";
        transform.position = _position;
        transform.localScale = new Vector3(0.5f, 0.5f, 1);

        rig.gravityScale = 1;
        coroutine = StartCoroutine(DropItem());
    }

    IEnumerator DropItem()
    {
        float Angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle));
        Vector2 force = dir * 5f; //¹æÇâ ¼³Á¤ ÈÄ ·£´ý Èû¸¸Å­ ½ð´Ù

        rig.AddForce(force,ForceMode2D.Impulse); //ÈûÀ» ÁØ´Ù

        collider2D.enabled = true;
        collider2D.isTrigger = false;

        float time = 0;

        while (Vector2.Distance(startPosition,transform.position) < 2.5f)
        {
            sortingOrderGroup.UpdateSortingOrderGroup();
            time += Time.deltaTime;
            if(time > 2f)
            {
                break;
            }
            yield return null;
        }

        collider2D.enabled = false;
        collider2D.isTrigger = true;
        gameObject.tag = "DropedItem";
        spriteRenderer.sprite = item.Item_sprite;
        animator.SetBool("Idle", true);
        rig.gravityScale = 0;
        rig.velocity = Vector2.zero;

        while (transform.localScale.x <= 1f)
        {
            transform.localScale += new Vector3(1,1,0) * Time.deltaTime * 2f;
            yield return null;
        }

        collider2D.enabled = true;
    }

    public void Interact()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        gameObject.tag = "Untagged";
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
            transform.position += dir.normalized * 7f * Time.deltaTime;
            sortingOrderGroup.UpdateSortingOrderGroup();
            yield return null;
        }


        GameManager.Instance.player.GetComponent<Player>().inventory.GetItem(item, amount);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["GetItem"]);
        offObject();
    }

    public void offObject()
    {
        collider2D.enabled = false;
        rig.velocity = Vector2.zero;
        gameObject.tag = "Untagged";
        gameObject.SetActive(false);
    }
}
