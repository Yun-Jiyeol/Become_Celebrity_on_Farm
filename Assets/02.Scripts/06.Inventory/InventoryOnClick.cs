using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOnClick : MonoBehaviour
{
    [Header("������ ������ ��ȣ")]
    public int pageIndex;

    public void OnClickTab()
    {
        InventoryManager.Instance.SelectPage(pageIndex);
    }
}
