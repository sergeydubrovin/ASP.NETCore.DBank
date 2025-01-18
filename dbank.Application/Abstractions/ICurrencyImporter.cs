using dbank.Application.Models.Сurrencies;

namespace dbank.Application.Abstractions;

public interface ICurrencyImporter
{
    Task<ValuteItemDto> ImportByCurrencyCode(string currencyCode);
    Task<ValuteDto> Import();
}
