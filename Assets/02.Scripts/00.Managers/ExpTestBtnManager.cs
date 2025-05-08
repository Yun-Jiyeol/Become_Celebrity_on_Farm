using UnityEngine;

public class ExpTestBtnManager : MonoBehaviour
{
    public ExpManager expManager;

    public void OnClickAddExp()
    {
        if (expManager != null)
        {
            expManager.AddExp(10); // 테스트용 경험치
        }
        else
        {
            Debug.LogWarning("ExpManager가 연결되지 않았습니다.");
        }
    }
}