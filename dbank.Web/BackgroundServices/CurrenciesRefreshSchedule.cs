using DBank.Application.Abstractions;
using DBank.Domain.Options;
using Hangfire;
using Microsoft.Extensions.Options;

namespace DBank.Web.BackgroundServices;

public class CurrenciesRefreshSchedule(IRecurringJobManager recurringJobManager, IServiceScopeFactory scopeFactory,
                                       IOptions<RedisOptions> rOptions) : IHostedService
{
    private readonly string _cron = rOptions.Value.RefreshCacheHoursCron;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        recurringJobManager.AddOrUpdate("RefreshCache", () => RefreshCache(), _cron);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        recurringJobManager.RemoveIfExists("RefreshCache");

        return Task.CompletedTask;
    }
    
    public async Task RefreshCache()
    {
        using var scope = scopeFactory.CreateScope();
        var currenciesService = scope.ServiceProvider.GetRequiredService<ICurrenciesService>();
        await currenciesService.RefreshCurrencyCache();
    }
}
