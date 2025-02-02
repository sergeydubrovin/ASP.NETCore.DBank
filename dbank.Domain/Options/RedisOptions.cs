namespace DBank.Domain.Options;

public class RedisOptions
{
    /// <summary>
    /// Время жизни кэша в часах.
    /// </summary>
    public required int CacheLifeTimeHours { get; init; }
    
    /// <summary>
    /// Cron-выражение для обновления кэша.
    /// </summary>
    public required string RefreshCacheHoursCron { get; init; }

    public required List<string> SupportedCurrencies { get; init; }
    
}
