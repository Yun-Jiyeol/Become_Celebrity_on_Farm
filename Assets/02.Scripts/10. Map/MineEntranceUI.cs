using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// MineEntranceUI에 붙일 스크립트
/// 광산 종류 선택 담당
/// </summary>
public class MineEntranceUI : MonoBehaviour
{
    [SerializeField] private MapTransition mineEntrance;

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

    public void OpenUI()
    {
        this.gameObject.SetActive(true);
    }

    void OnStoneButtonClick()
    {
        mineEntrance.LoadSelectedMine(MapType.StoneMine);
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnCopperButtonClick()
    {
        mineEntrance.LoadSelectedMine(MapType.CopperMine);
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnIronButtonClick()
    {
        mineEntrance.LoadSelectedMine(MapType.IronMine);
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnCloseButtonClick()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
