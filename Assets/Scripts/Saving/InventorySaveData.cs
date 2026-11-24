using System;
using System.Collections.Generic;
[Serializable]
public class InventorySaveData
{
    public int money;


    public List<InventoryItemSaveData> sewingItems = new List<InventoryItemSaveData>();


    public List<InventoryItemSaveData> combiningItems = new List<InventoryItemSaveData>();

    public void EnsureCollections()
    {
        if (sewingItems == null)
            sewingItems = new List<InventoryItemSaveData>();

        if (combiningItems == null)
            combiningItems = new List<InventoryItemSaveData>();
    }
}
