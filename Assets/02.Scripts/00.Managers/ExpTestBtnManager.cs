using UnityEngine;

public class ExpTestBtnManager : MonoBehaviour
{
    public ExpManager expManager;

    public void OnClickAddExp()
    {
        if (expManager != null)
        {
            expManager.AddExp(10); // �׽�Ʈ�� ����ġ
        }
        else
        {
            Debug.LogWarning("ExpManager�� ������� �ʾҽ��ϴ�.");
        }
    }
}