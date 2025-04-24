using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private Transform slotParent;             // ���Ե��� �� �θ�
    [SerializeField] private GameObject questSlotPrefab;       // ���� ������

    private List<GameObject> activeSlots = new List<GameObject>();
    private const int maxSlots = 3;

    public bool HasEmptySlot => activeSlots.Count < maxSlots;


    public void Assign(QuestData quest)
    {
        AddQuestSlot(quest);
    }

    /// <summary>
    /// ����Ʈ ���� �ʱ�ȭ (�ʱ⿣ �����)
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
    /// ���ο� ����Ʈ�� ���Կ� �߰��� (�ִ� 3�� ����)
    /// </summary>
    public void AddQuestSlot(QuestData quest)
    {
        if (activeSlots.Count >= maxSlots)
        {
            Debug.Log("[QuestSlotUI] ����Ʈ ������ ���� á���ϴ�.");
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
