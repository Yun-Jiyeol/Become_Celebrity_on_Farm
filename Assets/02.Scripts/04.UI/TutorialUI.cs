using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject firstPage;
    [SerializeField] private GameObject secondPage;
    public GameObject thirdPage;
    public GameObject fourthPage;
    [SerializeField] private GameObject fifthPage;
    public GameObject sixthPage;
    public GameObject seventhPage;
    [SerializeField] private GameObject eighthPage;

    readonly Dictionary<int, GameObject> pages = new();

    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button closeButton;

    int curPage = 0;

    void Start()
    {
        SetPage();

        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        firstPage.SetActive(true);
        secondPage.SetActive(false);
        thirdPage.SetActive(false);
        fourthPage.SetActive(false);
        fifthPage.SetActive(false);
        sixthPage.SetActive(false);
        seventhPage.SetActive(false);
        eighthPage.SetActive(false);
    }

    void OnEnable()
    {
        if (GameManager.Instance == null || GameManager.Instance.player == null)  return;
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.enabled = false;
    }

    void OnDisable()
    {
        if (GameManager.Instance.player == null) return;
        if (GameManager.Instance.player.TryGetComponent(out PlayerInput input)) input.enabled = true;
        input.actions["Click"].Enable();
    }

    void SetPage()
    {
        pages[0] = firstPage;
        pages[1] = secondPage;
        pages[2] = thirdPage;
        pages[3] = fourthPage;
        pages[4] = fifthPage;
        pages[5] = sixthPage;
        pages[6] = seventhPage;
        pages[7] = eighthPage;
    }

    void OnClickLeftButton()
    {
        MovePreviousPage(curPage);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["GetItem"]);
    }

    void OnClickRightButton()
    {
        MoveNextPage(curPage);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["GetItem"]);
    }

    void OnClickCloseButton()
    {
        ResetPage();
        this.gameObject.SetActive(false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["GetItem"]);
    }

    /// <summary>
    /// 다음 페이지로 넘기기
    /// </summary>
    void MoveNextPage(int cur)
    {
        switch (cur)
        {
            case 0: // 현재 페이지가 첫번째라면 두번째로
                secondPage.SetActive(true);
                firstPage.SetActive(false);
                pageText.text = "2/8";
                curPage++;
                break;
            case 1: // 현재 페이지가 두번째라면 세번째로
                thirdPage.SetActive(true);
                secondPage.SetActive(false);
                curPage++;
                pageText.text = "3/8";
                break;
            case 2: // 현재 페이지가 세번째라면 네번째로
                fourthPage.SetActive(true);
                thirdPage.SetActive(false);
                curPage++;
                pageText.text = "4/8";
                break;
            case 3: // 현재 페이지가 네번째라면 다섯번째로
                fifthPage.SetActive(true);
                fourthPage.SetActive(false);
                curPage++;
                pageText.text = "5/8";
                break;
            case 4: 
                sixthPage.SetActive(true);
                fifthPage.SetActive(false);
                curPage++;
                pageText.text = "6/8";
                break;
            case 5: 
                seventhPage.SetActive(true);
                sixthPage.SetActive(false);
                curPage++;
                pageText.text = "7/8";
                break;
            case 6: 
                eighthPage.SetActive(true);
                seventhPage.SetActive(false);
                curPage++;
                pageText.text = "8/8";
                break;
            case 7:
                break;
        }
    }

    /// <summary>
    /// 이전 페이지로 넘기기
    /// </summary>
    void MovePreviousPage(int cur)
    {
        switch (cur)
        {
            case 0: // 현재 페이지가 첫번째라면 리턴
                break;
            case 1: // 현재 페이지가 두번째라면 첫번째로
                firstPage.SetActive(true);
                secondPage.SetActive(false);
                curPage--;
                pageText.text = "1/8";
                break;
            case 2: // 현재 페이지가 세번째라면 두번째로
                secondPage.SetActive(true);
                thirdPage.SetActive(false);
                curPage--;
                pageText.text = "2/8";
                break;
            case 3: // 현재 페이지가 네번째라면 세번째로
                thirdPage.SetActive(true);
                fourthPage.SetActive(false);
                curPage--;
                pageText.text = "3/8";
                break;
            case 4: // 현재 페이지가 다섯번째라면 네번째로
                fourthPage.SetActive(true);
                fifthPage.SetActive(false);
                curPage--;
                pageText.text = "4/8";
                break;
            case 5: // 현재 페이지가 다섯번째라면 네번째로
                fifthPage.SetActive(true);
                sixthPage.SetActive(false);
                curPage--;
                pageText.text = "5/8";
                break;
            case 6: // 현재 페이지가 다섯번째라면 네번째로
                sixthPage.SetActive(true);
                seventhPage.SetActive(false);
                curPage--;
                pageText.text = "6/8";
                break;
            case 7: // 현재 페이지가 다섯번째라면 네번째로
                seventhPage.SetActive(true);
                eighthPage.SetActive(false);
                curPage--;
                pageText.text = "7/8";
                break;
        }
    }

    /// <summary>
    /// 1페이지 고정
    /// </summary>
    void ResetPage()
    {
        curPage = 0;
        pageText.text = "1/8";
        firstPage.gameObject.SetActive(true);

        for (int i = 1; i < pages.Count; i++)
        {
            if (pages.TryGetValue(i, out GameObject page))
                page.SetActive(false);
        }
    }
}
