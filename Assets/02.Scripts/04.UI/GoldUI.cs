using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GoldUI : UIBase
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start() //���� ���۽� ��� �̺�Ʈ ����, ���� ���� ui�ʱ�ȭ
    {
        if(GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(GoldManager.Instance.CurGold); // ���� �� �ʱ�ȭ
        }
    }

    private void OnDestroy() //������Ʈ�� ���ŵɶ� �̺�Ʈ ���� ����
    {
        if(GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int gold) //��� ���� �޾� �ؽ�Ʈ�� �ݿ�
    {
        goldText.text = gold.ToString("0000000");
    }

    public override void Show()
    {
        base.Show();
        UpdateUI(GoldManager.Instance.CurGold);
    }
}
