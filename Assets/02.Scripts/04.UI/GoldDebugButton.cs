using UnityEngine;
using UnityEngine.UI;

public class GoldDebugButton : MonoBehaviour
{
    public Button addGoldButton;
    public Button minusGoldButton;

    private void Start()
    {
        addGoldButton.onClick.AddListener(() =>
        {
            GoldManager.Instance.AddGold(100);
        });
        minusGoldButton.onClick.AddListener(() =>
        {
            GoldManager.Instance.SpendGold(100);
        });
    }
}