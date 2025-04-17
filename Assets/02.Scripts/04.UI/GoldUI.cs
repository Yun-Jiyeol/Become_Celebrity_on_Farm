using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GoldUI : UIBase
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start() //게임 시작시 골드 이벤트 구독, 현재 골드로 ui초기화
    {
        if(GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(GoldManager.Instance.CurGold); // 시작 시 초기화
        }
    }

    private void OnDestroy() //오브젝트가 제거될때 이벤트 구독 해제
    {
        if(GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int gold) //골드 값을 받아 텍스트에 반영
    {
        goldText.text = gold.ToString("0000000");
    }

    public override void Show()
    {
        base.Show();
        UpdateUI(GoldManager.Instance.CurGold);
    }
}
