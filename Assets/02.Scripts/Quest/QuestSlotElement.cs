using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestSlotElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void SetData(QuestData quest)
    {
        titleText.text = quest.questTitle;
        descriptionText.text = quest.questDescription;
    }
}