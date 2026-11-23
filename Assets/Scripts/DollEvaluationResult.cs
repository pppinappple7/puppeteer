public sealed class DollEvaluationResult
{
    public bool IsValid { get; private set; }
    public bool IsGood { get; private set; }
    public int Payment { get; private set; }
    public int MatchedRequirements { get; private set; }
    public int TotalRequirements { get; private set; }
    public float MatchPercent { get; private set; }
    public string Error { get; private set; }

    private DollEvaluationResult() { }

    public static DollEvaluationResult Success(
        bool isGood,
        int payment,
        int matchedRequirements,
        int totalRequirements,
        float matchPercent)
    {
        return new DollEvaluationResult
        {
            IsValid = true,
            IsGood = isGood,
            Payment = payment,
            MatchedRequirements = matchedRequirements,
            TotalRequirements = totalRequirements,
            MatchPercent = matchPercent,
            Error = string.Empty
        };
    }

    public static DollEvaluationResult Failure(string error)
    {
        return new DollEvaluationResult
        {
            IsValid = false,
            IsGood = false,
            Payment = 0,
            MatchedRequirements = 0,
            TotalRequirements = 0,
            MatchPercent = 0f,
            Error = error
        };
    }
}
