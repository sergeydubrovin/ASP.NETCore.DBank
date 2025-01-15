using dbank.Application.Models.Payments;
using dbank.Domain.Entities;
using dbank.Domain.Exceptions;

namespace dbank.Application.Services.Payments;

public static class CorePaymentsService
{
    public static void ValidationPayment(this CreatePaymentDto? payment, CustomerEntity? sender,
                                                    CustomerEntity? recipient)
    {
        switch (sender, recipient)
        {
            case (null, _):
                throw new EntityNotFoundException($"Отправитель с id {payment!.CustomerId} не найден.");

            case (_, null):
                throw new EntityNotFoundException(
                    $"Получатель с номером карты {payment!.RecipientCardNumber} не найден.");

            case ({ Balance: null }, _):
                throw new EntityNotFoundException($"У отправителя с id {payment!.CustomerId} не открыт баланс.");

            case ({ Balance: { Balance: var senderBalance } }, _)
                when senderBalance < payment!.PaymentAmount:
                throw new EntityNotFoundException("На балансе недостаточно средств.");
        }
    }
}
