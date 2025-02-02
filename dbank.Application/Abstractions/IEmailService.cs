namespace DBank.Application.Abstractions;

public interface IEmailService
{
    Task SendEmail(string email, string subject, string message);
    string GenerateCode();
}
