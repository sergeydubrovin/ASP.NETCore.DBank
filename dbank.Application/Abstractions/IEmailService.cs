using DBank.Application.Models.Email;

namespace DBank.Application.Abstractions;

public interface IEmailService
{
    Task SendEmail(CreateEmailMessage emailMessage);
    string GenerateCode();
}
