using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    GameObject tutorialUI;
    [SerializeField] private GameObject firstPage;
    [SerializeField] private GameObject secondPage;
    [SerializeField] private GameObject thirdPage;
    [SerializeField] private GameObject fourthPage;
    private Dictionary<int, GameObject> pages = new Dictionary<int, GameObject>();

    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button closeButton;

    int curPage = 0;

    void Start()
    {
        SetPage();
        tutorialUI = this.gameObject;

        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        firstPage.SetActive(true);
        secondPage.SetActive(false);
        thirdPage.SetActive(false);
        fourthPage.SetActive(false);

        //tutorialUI.SetActive(false);
    }

    void SetPage()
    {
        pages[0] = firstPage;
        pages[1] = secondPage;
        pages[2] = thirdPage;
        pages[3] = fourthPage;
    }

    void OnClickLeftButton()
    {
        MovePreviousPage(curPage);
    }

    void OnClickRightButton()
    {
        MoveNextPage(curPage);
    }

    void OnClickCloseButton()
    {
        tutorialUI.SetActive(false);
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
                pageText.text = "2/4";
                curPage++;
                break;
            case 1: // 현재 페이지가 두번째라면 세번째로
                thirdPage.SetActive(true);
                secondPage.SetActive(false);
                curPage++;
                pageText.text = "3/4";
                break;
            case 2: // 현재 페이지가 세번째라면 네번째로
                fourthPage.SetActive(true);
                thirdPage.SetActive(false);
                curPage++;
                pageText.text = "4/4";
                break;
            case 3:
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
                pageText.text = "1/4";
                break;
            case 2: // 현재 페이지가 세번째라면 두번째로
                secondPage.SetActive(true);
                thirdPage.SetActive(false);
                curPage--;
                pageText.text = "2/4";
                break;
            case 3: // 현재 페이지가 네번째라면 세번째로
                thirdPage.SetActive(true);
                fourthPage.SetActive(false);
                curPage--;
                pageText.text = "3/4";
                break;
        }
    }
}
