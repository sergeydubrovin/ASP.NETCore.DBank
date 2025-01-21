using DBank.Application.Models.Currencies;

namespace DBank.Application.Abstractions;

public interface ICurrenciesImporter
{
    public Task<ValuteDto> Import();
}
