using DBank.Application.Models.Currencies;

namespace DBank.Application.Abstractions;

public interface ICurrenciesImporter
{ 
    Task<ValuteDto> Import();
}
