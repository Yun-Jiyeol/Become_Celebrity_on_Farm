using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private Transform slotParent;             // 슬롯들이 들어갈 부모
    [SerializeField] private GameObject questSlotPrefab;       // 슬롯 프리팹

    private List<GameObject> activeSlots = new List<GameObject>();
    private const int maxSlots = 3;

    public bool HasEmptySlot => activeSlots.Count < maxSlots;


    public void Assign(QuestData quest)
    {
        AddQuestSlot(quest);
    }

    /// <summary>
    /// 퀘스트 슬롯 초기화 (초기엔 비워둠)
    /// </summary>
    public void ClearSlots()
    {
        foreach (var slot in activeSlots)
        {
            Destroy(slot);
        }
        activeSlots.Clear();
    }

    /// <summary>
    /// 새로운 퀘스트를 슬롯에 추가함 (최대 3개 제한)
    /// </summary>
    public void AddQuestSlot(QuestData quest)
    {
        if (activeSlots.Count >= maxSlots)
        {
            Debug.Log("[QuestSlotUI] 퀘스트 슬롯이 가득 찼습니다.");
            return;
        }

        GameObject newSlot = Instantiate(questSlotPrefab, slotParent);
        QuestSlotElement element = newSlot.GetComponent<QuestSlotElement>();
        if (element != null)
        {
            element.SetData(quest);
        }

        activeSlots.Add(newSlot);
    }
}
