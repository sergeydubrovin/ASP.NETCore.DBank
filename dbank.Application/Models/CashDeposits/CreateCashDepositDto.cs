using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace dbank.Application.Models.CashDeposits;

public class CreateCashDepositDto
{
    public long? CustomerId { get; set; }
    public string? DepositName { get; set; }
    
    [Required(ErrorMessage = "Данное поле обязательно для заполнения.")]
    [Range(100000, double.MaxValue, ErrorMessage = "Минимальная сумма вклада 100.000Р!")]
    public decimal? DepositAmount { get; set; }
    
    [Required(ErrorMessage = "Данное поле обязательно для заполнения.")]
    [Range(3, 36, ErrorMessage = "Минимальный срок 3 месяца, максимальный 36!")]
    [DefaultValue(3)]
    public decimal? DepositPeriod { get; set; }
    
    [Required(ErrorMessage = "Процентная ставка на данный момент составляет 21%.")]
    [Range(0.21, 0.21, ErrorMessage = "Изменение ставки исключено.")]
    [DefaultValue(0.21)]
    public decimal? InterestRate { get; set; }
}
