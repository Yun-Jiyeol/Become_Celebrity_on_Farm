using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoGetItem : MonoBehaviour
{
    CircleCollider2D collider;
    public List<GameObject> NPCs;

    public GameObject ClosestNPC;
    float lastCheckTime = 0;

    private void Start()
    {
        collider = GetComponent<CircleCollider2D>();

        settingGetItemRange();
    }

    private void Update()
    {
        if(NPCs.Count != 0 && lastCheckTime + 0.2f <= Time.time)
        {
            lastCheckTime = Time.time;
            CheckClosestNPC();
            ClosestNPC.GetComponent<NPCData>().Choosed.SetActive(true);
        }
        else if(NPCs.Count == 0)
        {
            ClosestNPC = null;
        }
    }

    void CheckClosestNPC()
    {
        float shortest = 10;

        for (int i = 0; i < NPCs.Count; i++)
        {
            NPCs[i].GetComponent<NPCData>().Choosed.SetActive(false);

            float distance = Vector2.Distance(gameObject.transform.position, NPCs[i].transform.position);
            if (shortest > distance)
            {
                shortest = distance;
                ClosestNPC = NPCs[i];
            }
        }
    }

    public void settingGetItemRange()
    {
        collider.radius = gameObject.GetComponentInParent<Player>().stat.GetItemRange * 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "DropedItem")
        {
            collision.GetComponent<IInteract>().Interact();
        }
        else if(collision.transform.tag == "NPC")
        {
            if (!NPCs.Contains(collision.gameObject))
            {
                NPCs.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "NPC")
        {
            if (NPCs.Contains(collision.gameObject))
            {
                collision.gameObject.GetComponent<NPCData>().Choosed.SetActive(false);
                NPCs.Remove(collision.gameObject);
            }
        }
    }
}
