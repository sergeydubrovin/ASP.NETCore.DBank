using System.Security.Cryptography;
using DBank.Application.Abstractions;
using DBank.Application.Models.Email;
using MailKit.Net.Smtp;
using DBank.Domain.Options;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DBank.Application.Services;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _senderEmail;
    private readonly string _senderPassword;
    
    private readonly RandomNumberGenerator _generator;
    private readonly string _charsGenerate;
    private readonly int _lengthGenerate;

    public EmailService(IOptions<EmailOptions> options)
    {
        var emailOptions = options.Value;
        _smtpServer = emailOptions.SmtpServer;
        _smtpPort = emailOptions.SmtpPort;
        _senderEmail = emailOptions.SenderEmail;
        _senderPassword = emailOptions.SenderPassword;

        _charsGenerate = emailOptions.CharsGenerateCode;
        _lengthGenerate = emailOptions.LengthGenerateCode;
        _generator = RandomNumberGenerator.Create();
    }

    public async Task SendEmail(CreateEmailMessage emailMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("DBank", _senderEmail));
        message.To.Add(MailboxAddress.Parse(emailMessage.RecipientEmail));
        message.Subject = emailMessage.Subject;
        message.Body = new TextPart{ Text = emailMessage.Body };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_senderEmail, _senderPassword);
            await client.SendAsync(message);
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not send email. Error: {ex.Message}");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    public string GenerateCode()
    {
        var resultCode = new char[_lengthGenerate];
        var bytes = new byte[_lengthGenerate];
        _generator.GetBytes(bytes);
        
        for (var i = 0; i < _lengthGenerate; i++)
        {
            resultCode[i] = _charsGenerate[bytes[i] % _charsGenerate.Length];
        }

        return new string(resultCode);
    }
}
