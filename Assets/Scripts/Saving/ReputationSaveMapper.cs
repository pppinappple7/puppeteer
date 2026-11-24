public static class ReputationSaveMapper
{
    public static bool TryCreate(
        Reputation reputation,
        out ReputationSaveData saveData,
        out string error)
    {
        saveData = null;
        error = null;

        if (reputation == null)
        {
            error = "Reputation is not assigned.";
            return false;
        }

        if (reputation.Points < 0)
        {
            error = "Reputation points cannot be negative.";
            return false;
        }

        saveData = new ReputationSaveData
        {
            points = reputation.Points
        };

        return true;
    }

    public static bool TryRestore(
        Reputation reputation,
        ReputationSaveData saveData,
        out string error)
    {
        error = null;

        if (reputation == null)
        {
            error = "Reputation is not assigned.";
            return false;
        }

        if (saveData == null)
        {
            error = "Reputation save data is missing.";
            return false;
        }

        return reputation.TrySetPoints(saveData.points, out error);
    }
}
