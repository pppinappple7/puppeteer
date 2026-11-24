using System.Collections.Generic;

public static class ShiftSaveMapper
{
    public static bool TryCreate(
        ShiftManager shiftManager,
        out ShiftSaveData saveData,
        out string error)
    {
        saveData = null;
        error = null;

        if (shiftManager == null)
        {
            error = "ShiftManager is not assigned.";
            return false;
        }

        List<string> usedCustomerIds = new List<string>();
        if (!shiftManager.TryGetUsedCustomerIds(usedCustomerIds, out error))
            return false;

        saveData = new ShiftSaveData
        {
            completedShifts = shiftManager.CompletedShifts,
            usedCustomerIds = usedCustomerIds
        };

        return true;
    }

    public static bool TryRestore(
        ShiftManager shiftManager,
        ShiftSaveData saveData,
        out string error)
    {
        error = null;

        if (shiftManager == null)
        {
            error = "ShiftManager is not assigned.";
            return false;
        }

        if (saveData == null)
        {
            error = "Shift save data is missing.";
            return false;
        }

        saveData.EnsureCollections();
        return shiftManager.TryRestoreProgress(saveData.completedShifts, saveData.usedCustomerIds, out error);
    }
}
