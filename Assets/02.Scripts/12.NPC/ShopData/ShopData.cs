using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SellingCatalog
{
    public int ItemData_num;
    public int Price;
}

[CreateAssetMenu(fileName = "ShopData", menuName = "NPC/ShopData")]
public class ShopData : ScriptableObject
{
    public SellingCatalog[] BaseCatalogs;
    public SellingCatalog[] SpringCatalogs;
    public SellingCatalog[] SummerCatalogs;
    public SellingCatalog[] FallCatalogs;
    public SellingCatalog[] WinterCatalogs;
}
