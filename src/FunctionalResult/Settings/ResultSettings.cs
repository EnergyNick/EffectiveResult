namespace FunctionalResult.Settings;

/// <summary>
/// Settings for general Result behavior in certain scenarios
/// </summary>
public static class ResultSettings
{
    /// <summary>
    /// Current parameters used in Results
    /// </summary>
    public static ResultParameters Current { get; private set; }

    static ResultSettings() => Current = GetDefaultParameters();

    /// <summary>
    /// Thread-safe parameters modification for results
    /// </summary>
    public static void SetupParameters(Func<ResultParameters, ResultParameters> settingsChangeFunction)
    {
        lock (Current)
        {
            Current = settingsChangeFunction(Current);
        }
    }

    /// <summary>
    /// Get predefined default value of parameters
    /// </summary>
    public static ResultParameters GetDefaultParameters() => new()
    {
        DefaultTryCatchHandler = ex => new ExceptionalError(ex)
    };
}