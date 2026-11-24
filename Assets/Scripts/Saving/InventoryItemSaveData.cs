using System;
using UnityEngine;


[Serializable]
public class InventoryItemSaveData
{
    public string itemId;
    public InventoryItemKind itemKind;


    public int amount = 1;


    public int price;


    public Color color1 = Color.white;
    public Color color2 = Color.white;
}


public enum InventoryItemKind
{
    SewingCloth = 0,
    SewingPart = 1,
    SewingClothes = 2,
    CombiningPart = 3,
    CombiningClothes = 4
}
