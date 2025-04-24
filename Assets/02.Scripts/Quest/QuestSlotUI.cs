using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> slotTexts; // ≈ÿΩ∫∆Æ ΩΩ∑‘µÈ (√÷¥Î 3∞≥)
    private List<QuestData> acceptedQuests = new List<QuestData>();

    public bool HasEmptySlot => acceptedQuests.Count < slotTexts.Count;

    public void Assign(QuestData quest)
    {
        if (!HasEmptySlot)
        {
            Debug.LogWarning("ƒ˘Ω∫∆Æ ΩΩ∑‘¿Ã ∞°µÊ √°Ω¿¥œ¥Ÿ.");
            return;
        }

        acceptedQuests.Add(quest);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotTexts.Count; i++)
        {
            if (i < acceptedQuests.Count)
            {
                slotTexts[i].text = $" {acceptedQuests[i].questTitle}";
            }
            else
            {
                slotTexts[i].text = ""; // ≥≤¿∫ ΩΩ∑‘¿∫ ∫Òøˆµ“
            }
        }
    }

    public void ClearAll()
    {
        acceptedQuests.Clear();
        UpdateUI();
    }
}