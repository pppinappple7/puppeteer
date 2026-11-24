using System;


[Serializable]
public class SaveData
{
    public int version = 1;

    public InventorySaveData inventory = new InventorySaveData();
    public ReputationSaveData reputation = new ReputationSaveData();
    public ShiftSaveData shift = new ShiftSaveData();

    
    public void EnsureStructure()
    {
        if (inventory == null)
            inventory = new InventorySaveData();
        inventory.EnsureCollections();

        if (reputation == null)
            reputation = new ReputationSaveData();

        if (shift == null)
            shift = new ShiftSaveData();
        shift.EnsureCollections();
    }
}
