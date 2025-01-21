using DBank.Application.Abstractions;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;

namespace DBank.Web.BackgroundServices;

public class CurrenciesBackground(IOptions<CbOptions> cbOptions, IServiceProvider sp) : BackgroundService
{
    private readonly CbOptions _cb = cbOptions.Value;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = sp.CreateScope();
            var currenciesService = scope.ServiceProvider.GetRequiredService<ICurrenciesService>();

            await currenciesService.UpdateCurrenciesCache();
            
            await Task.Delay(TimeSpan.FromMinutes(_cb.DelayMinutes), stoppingToken);
        }
    }
}
