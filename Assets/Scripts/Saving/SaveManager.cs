using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] ShopCatalog shopCatalog;
    [SerializeField] Reputation reputation;
    [SerializeField] ShiftManager shiftManager;

    public bool SaveLoaded { get; private set; }
    public bool IsInitialized { get; private set; }
    public bool HasLoadedSave { get; private set; }
    public string LastError { get; private set; }

    void Start()
    {
        Initialize();
    }

    public bool Initialize()
    {
        SaveLoaded = false;
        if (IsInitialized)
            return true;

        LastError = null;

        if (!SaveSystem.HasSave())
        {
            IsInitialized = true;
            HasLoadedSave = false;
            return true;
        }
        if (!SaveSystem.TryLoad(out SaveData data, out string loadError))
        {
            LastError = loadError;
            return false;
        }
        if (!TryRestore(data, out string restoreError))
        {
            LastError = restoreError;
            Debug.LogError("SAVE LOAD ERROR: " + LastError);
            return false;
        }
        SaveLoaded = true;
        IsInitialized = true;
        return true;
    }

    public bool TrySaveGame(out string error)
    {
        error = null;
        if (!IsInitialized)
        {
            error = LastError ?? "SaveManager has not initialized successfully.";
            LastError = error;
            return false;
            
        }
        SaveData data = new SaveData();

        if (!InventorySaveMapper.TryCreate(inventory, shopCatalog, out data.inventory, out error))
        {
            LastError = error;
            return false;

        }
        if (!ReputationSaveMapper.TryCreate(reputation, out data.reputation, out error))
        {
            LastError = error;
            return false;
        }
        if (!ShiftSaveMapper.TryCreate(shiftManager, out data.shift, out error))
        {
            LastError = error;
            return false;
        }
        if (!SaveSystem.TrySave(data, out error))
        {
            LastError = error;
            return false;
        }
        SaveLoaded=true;
        LastError = null;
        IsInitialized = true;
        HasLoadedSave = true;
        return true;
    }

    bool TryRestore(SaveData data, out string error)
    {
        error = null;

        if (data == null)
        {
            error = "Save data is missing.";
            return false;
        }

        data.EnsureStructure();

        if (!InventorySaveMapper.TryRestore(inventory, shopCatalog, data.inventory, out error))
        {
            error = "Inventory restore failed: " + error;
            return false;
        }

        if (!ReputationSaveMapper.TryRestore(reputation, data.reputation, out error))
        {
            error = "Reputation restore failed: " + error;
            return false;
        }

        if (!ShiftSaveMapper.TryRestore(shiftManager, data.shift, out error))
        {
            error = "Shift restore failed: " + error;
            return false;
        }

        return true;
    }
    [ContextMenu("Delete Save For Testing")]
    public void DeleteSaveForTesting()
    {
        if (!SaveSystem.DeleteSave(out string error))
        {
            LastError = error;
            return;
        }

        LastError = null;
    }
}
