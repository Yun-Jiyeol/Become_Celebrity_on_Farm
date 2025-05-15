using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeStump : MonoBehaviour, IHaveHP, IInteract
{
    public float HP { get; set; }
    public float MaxHP { get; set; }

    string nowsprite;
    private int WoodItemNum = 1;
    private int WoodItemAmount = 3;

    private void Start()
    {
        nowsprite = gameObject.GetComponent<SpriteRenderer>().sprite.name;
    }

    public void GetDamage(float amount)
    {
        HP += amount;

        if (HP <= 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["CutWood"]);
            ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[WoodItemNum], WoodItemAmount, gameObject.transform.position);
            Destroy(gameObject);
        }
        else if(amount <= 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["CutWood"]);
            StartCoroutine(DamageCoroutine());
        }
    }

    public void Interact()
    {
        //�÷��̾��� ����? ���ݷ¸� �޾ƿ� ������ ����ϴ� ������ �߰�

        GetDamage(-(GameManager.Instance.player.GetComponent<Player>().stat.Attack + GameManager.Instance.player.GetComponent<Player>().playerController.ItemDamage));
    }
    IEnumerator DamageCoroutine()
    {
        if (ResourceManager.Instance.splits["Damage_" + nowsprite] != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits["Damage_" + nowsprite];
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.splits[nowsprite];
    }
}
