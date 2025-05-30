using UnityEngine;
using UnityEngine.UI;

public class QuestPhone : MonoBehaviour
{
    [SerializeField] private Button phoneButton;          // 폰 버튼
    [SerializeField] private GameObject notificationIcon; // 느낌표 아이콘 같은 알림 표시

    private void Awake()
    {
        HideNotification();
    }

    private void OnEnable()
    {
        phoneButton.onClick.RemoveAllListeners(); // 중복 방지
        phoneButton.onClick.AddListener(OnPhoneClicked);
    }

    public void ShowNotification() => notificationIcon.SetActive(true);
    public void HideNotification() => notificationIcon.SetActive(false);  

    private void OnPhoneClicked()
    {
        Debug.Log("[QuestPhone] 유튜브 버튼 클릭됨");

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestButtonClicked();
        }
    }
}