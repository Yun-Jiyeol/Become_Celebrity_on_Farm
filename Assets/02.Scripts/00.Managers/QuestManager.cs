using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private List<QuestData> allQuestPool = new List<QuestData>();// ����Ʈ Ǯ
    [SerializeField] private QuestPopupUI popupUI;
    [SerializeField] private QuestPhone phone;
    [SerializeField] private QuestSlotUI questSlot; // �ִ� 3�� ����

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
        if (pendingQuest == null && questSlot.HasEmptySlot)
        {
            pendingQuest = GetRandomQuest();
            phone.ShowNotification();
        }
    }

    public void OnQuestButtonClicked()
    {
        if (pendingQuest != null)
        {
            popupUI.Show(pendingQuest); // ����/���� UI ����
        }
        else
        {
            popupUI.ShowNoQuest(); // ����Ʈ ���� �� �޽���
        }
    }

    public void AcceptQuest()
    {
        if (!questSlot.HasEmptySlot) return;

        questSlot.Assign(pendingQuest);
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
        Debug.Log($"[QuestManager] ����Ʈ Ǯ ����: {allQuestPool.Count}");
        return allQuestPool[Random.Range(0, allQuestPool.Count)];
    }
}
