using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOnClick : MonoBehaviour
{
    [Header("선택할 페이지 번호")]
    public int pageIndex;

    public void OnClickTab()
    {
        InventoryManager.Instance.SelectPage(pageIndex);
    }
}
