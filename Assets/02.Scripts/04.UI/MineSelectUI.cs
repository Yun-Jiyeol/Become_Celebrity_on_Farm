using UnityEngine;
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

        // 여기에서 mineEntrance 찾기
        //mineEntrance = FindObjectOfType<MapTransition>(); 이거 아닌 것 같음. 오브젝트 이름으로 찾아야 될듯

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
        mineEntrance.LoadSelectedMine(MapType.StoneMine);
        this.gameObject.SetActive(false);
    }

    void OnCopperButtonClick()
    {
        mineEntrance.LoadSelectedMine(MapType.CopperMine);
        this.gameObject.SetActive(false);
    }

    void OnIronButtonClick()
    {
        mineEntrance.LoadSelectedMine(MapType.IronMine);
        this.gameObject.SetActive(false);
    }

    void OnCloseButtonClick()
    {
        this.gameObject.SetActive(false);

        if (GameManager.Instance.player.TryGetComponent(out PlayerController controller))
            controller.enabled = true;
    }
}
