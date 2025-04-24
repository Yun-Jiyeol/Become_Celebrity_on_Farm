using UnityEngine;
using UnityEngine.UI;

public class QuestPhone : MonoBehaviour
{
    [SerializeField] private Button phoneButton;          // �� ��ư
    [SerializeField] private GameObject notificationIcon; // ����ǥ ������ ���� �˸� ǥ��

    private void Awake()
    {
        phoneButton.onClick.AddListener(OnPhoneClicked);
        HideNotification();
    }

    public void ShowNotification() => notificationIcon.SetActive(true);
    public void HideNotification() => notificationIcon.SetActive(false);  

    private void OnPhoneClicked()
    {
        Debug.Log("[QuestPhone] ��Ʃ�� ��ư Ŭ����");

        HideNotification();

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestButtonClicked();
        }
    }
}