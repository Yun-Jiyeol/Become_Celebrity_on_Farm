using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public List<GameObject> ListOfCraftingTable;
    public GameObject PlayerCraftTable;

    private void Start()
    {
        GameObject go = Instantiate(PlayerCraftTable, InventoryManager.Instance.pages[1].transform);
        go.GetComponent<CraftingScroll>().Setting();
    }
}
