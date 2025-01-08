using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace dbank.Application.Models.CashDeposits;

public class CreateCashDepositDto
{
    public long? CustomerId { get; set; }
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Данное поле обязательно для заполнения!")]
    [Range(100000, double.MaxValue, ErrorMessage = "Минимальная сумма вклада 100.000Р!")]
    public decimal? DepositAmount { get; set; }
    
    [Required(ErrorMessage = "Данное поле обязательно для заполнения!")]
    [Range(3, 36,  ErrorMessage = "Минимальный срок 3 месяца, максимальный 36!")]
    [DefaultValue(3)]
    public decimal? DepositPeriod { get; set; }
    
    public decimal? InterestRate { get; set; } = 0.21m;
}
