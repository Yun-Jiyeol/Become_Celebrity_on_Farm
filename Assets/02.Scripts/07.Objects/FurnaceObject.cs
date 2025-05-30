using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class FurnaceObject : InteractedObject, IInteractNum
{
    public GameObject Furnace;
    public GameObject AfterWork;
    public SpriteRenderer AfterItem;

    int nowTime = 0;
    int targetTime = 0;
    bool isend = false;
    bool iswork = false;
    TimeManager timeManager;

    ConnectedObjectAndTime cot;
    int cotAfteritem = 0;

    private void Start()
    {
        AfterWork.SetActive(false);
        timeManager = TimeManager.Instance;
    }

    public override void Interact()
    {
        if (!isend) return;
        
        AfterWork.SetActive(false);
        ItemManager.Instance.spawnItem.DropItem(ItemManager.Instance.itemDataReader.itemsDatas[cotAfteritem], 1, gameObject.transform.position);
        isend = false;
    }

    public void Interact(int num)
    {
        cot = FindObject(num);
        if (cot == null) return;
        if (iswork) return;
        if(!GameManager.Instance.player.GetComponent<Player>().playerController.UseItemOnHand(cot.Amount)) return;

        iswork = true;

        nowTime = CheckNowTime();
        targetTime = (int)cot.Time * 60 + nowTime;

        Debug.Log(nowTime);
        Debug.Log(targetTime);


        Furnace.GetComponent<Animator>().SetBool("Burning", true);
        gameObject.GetComponent<AudioSource>().Play();
        timeManager.OnTimeChanged += checkTime;
    }

    void checkTime()
    {
        nowTime = CheckNowTime();
        if (targetTime <= nowTime)
        {
            iswork = false;
            isend = true;
            Furnace.GetComponent<Animator>().SetBool("Burning", false);
            gameObject.GetComponent<AudioSource>().Stop();
            AfterWork.SetActive(true);
            AfterItem.sprite = ItemManager.Instance.itemDataReader.itemsDatas[cot.AfterItemNum].Item_sprite;
            cotAfteritem = cot.AfterItemNum;
            timeManager.OnTimeChanged -= checkTime;
        }
    }

    int CheckNowTime()
    {
        TimeManager time = TimeManager.Instance;
        int _nowtime = time.currentMinute + 60 * (time.currentHour + 24*(time.currentDay + 7 * (time.currentMonth + 4 * (time.currentYear))));
        return _nowtime;
    }
}
