using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DBank.Application.Models.Credits;

public class CreateCreditDto
{
    public long? CustomerId { get; set; }
    
    [Required(ErrorMessage = "Данное поле обязательно для заполнения.")]
    [Range(1000000, 10000000, ErrorMessage = "Минимальная сумма кредита 1млн, максимальная 10млн рублей.")]
    [DefaultValue(1000000)]
    public decimal CreditAmount { get; set; }
    
    [Required(ErrorMessage = "Данное поле поле обязательно для заполнения.")]
    [Range(1, 10, ErrorMessage = "Минимальный срок кредита 1 год, максимальный 10 лет.")]
    [DefaultValue(1)]
    public int? CreditPeriod { get; set; }
    
    [Required(ErrorMessage = "Процентная ставка на данный момент составляет 31%.")]
    [Range(0.31, 0.31, ErrorMessage = "Изменение ставки исключено.")]
    public decimal InterestRate { get; set; }
    
    [Required(ErrorMessage = "Процентная ставка первоначального взноса составляет 28%.")]
    [Range(0.28, 0.28, ErrorMessage = "Изменение ставки исключено.")]
    public decimal InitialPaymentRate { get; set; }
}
