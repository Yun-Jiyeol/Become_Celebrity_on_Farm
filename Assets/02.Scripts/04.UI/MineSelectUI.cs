using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>
/// MineEntranceUI에 붙일 스크립트
/// 광산 종류 선택 담당
/// </summary>
public class MineSelectUI : UIBase
{
    MapTransition mineEntrance;

    [Header("UI")]
    [SerializeField] private Button stoneButton;
    [SerializeField] private Button copperButton;
    [SerializeField] private Button ironButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        this.gameObject.SetActive(false);

        stoneButton.onClick.AddListener(OnStoneButtonClick);
        copperButton.onClick.AddListener(OnCopperButtonClick);
        ironButton.onClick.AddListener(OnIronButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    public override void Show()
    {
        base.Show();
    }

    public void SetMineEntrance(MapTransition entrance)
    {
        mineEntrance = entrance;
    }

    void OnStoneButtonClick()
    {
        mineEntrance.SelectMine(MapType.StoneMine);
        this.gameObject.SetActive(false);
    }

    void OnCopperButtonClick()
    {
        mineEntrance.SelectMine(MapType.CopperMine);
        this.gameObject.SetActive(false);
    }

    void OnIronButtonClick()
    {
        mineEntrance.SelectMine(MapType.IronMine);
        this.gameObject.SetActive(false);
    }

    void OnCloseButtonClick()
    {
        this.gameObject.SetActive(false);

        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input))
            input.enabled = true;
    }
}
