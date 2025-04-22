using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private List<QuestData> allQuestPool; // ����Ʈ Ǯ
    [SerializeField] private QuestPopupUI popupUI;
    [SerializeField] private QuestPhone phone;
    [SerializeField] private List<QuestSlotUI> questSlots; // �ִ� 3�� ����

    private float questInterval = 120f; // ���� �ð� 2��
    private float timer;
    private QuestData pendingQuest;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= questInterval)
        {
            timer = 0f;
            TryGenerateQuest();
        }
    }

    void TryGenerateQuest()
    {
        if (pendingQuest == null)
        {
            pendingQuest = GetRandomQuest();
            phone.ShowNotification(); // ����Ʈ (��) �˸�
        }
    }

    public void OnQuestButtonClicked()
    {
        if (pendingQuest != null)
        {
            popupUI.Show(pendingQuest); // ����/���� UI ����
        }
    }

    public void AcceptQuest()
    {
        foreach (var slot in questSlots)
        {
            if (!slot.HasQuest)
            {
                slot.Assign(pendingQuest);
                break;
            }
        }
        pendingQuest = null;
        phone.HideNotification();
    }

    public void DeclineQuest()
    {
        pendingQuest = null;
        phone.HideNotification();
    }

    QuestData GetRandomQuest()
    {
        return allQuestPool[UnityEngine.Random.Range(0, allQuestPool.Count)];
    }
}
