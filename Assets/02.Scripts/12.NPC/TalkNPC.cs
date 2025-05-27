using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkNPC : NPCData, IInteract
{
    List<int> DailyTalk = new List<int>();
    List<int> RefeatTalk = new List<int>();

    TextUIManager textuimanager;
    List<NPCTextSave> npctextsaves = new List<NPCTextSave>();
    int nowtalksave = 0;
    Coroutine talkcoroutine;

    private void Start()
    {
        textuimanager = UIManager.Instance.textUIManager;
        npctextsaves = textuimanager.calledNPCText[npcName];
        SettingOneDay();
        TimeManager.Instance.OnDayChanged += SettingOneDay;
    }

    public void SettingOneDay()
    {
        DailyTalk.Clear();
        RefeatTalk.Clear();

        for (int i = 0; i < npctextsaves.Count; i++)
        {
            if(LikeGauge >= npctextsaves[i].MinLike && LikeGauge <= npctextsaves[i].MaxLike)
            {
                if(npctextsaves[i].DailyOrRefeat == "Daily")
                {
                    DailyTalk.Add(i);
                }
                else
                {
                    RefeatTalk.Add(i);
                }
            }
        }
    }

    public void Interact()
    {
        if(DailyTalk.Count == 0 && RefeatTalk.Count == 0) return;

        if (DailyTalk.Count > 0)
        {
            nowtalksave = DailyTalk[0];
            DailyTalk.Remove(DailyTalk[0]);
        }
        else
        {
            nowtalksave = RefeatTalk[0];
        }

        // 일퀘 7일차 인어 퀘스트 진행 보고용
        if (npcName == NPCName.FishingGuide)
        {
            PlannerQuestManager.Instance?.ReportAction("Talk");
        }

        if (ShopData != null)
        {
            UIManager.Instance.shopUIManager.lastshopData = ShopData;
        }

        LikeGauge += UIManager.Instance.textUIManager.calledNPCText[npcName][nowtalksave].AddLike;

        if (talkcoroutine != null) StopCoroutine(talkcoroutine);
        talkcoroutine = StartCoroutine(talkTextCoroutine());
    }

    IEnumerator talkTextCoroutine()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["TextStart"]);
        textuimanager.ShowTextUI(npcName, nowtalksave);

        if (npctextsaves[nowtalksave].canClick)
        {
            yield return new WaitForSeconds(0.2f);

            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Interact();
                }
                yield return null;
            }
        }
    }
}
