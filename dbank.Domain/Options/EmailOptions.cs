namespace DBank.Domain.Options;

public class EmailOptions
{
    public required string SmtpServer { get; init; }
    public required int SmtpPort { get; init; }
    public required string SenderEmail { get; init; }
    public required string SenderPassword { get; init; }
    
    public required string CharsGenerateCode { get; init; }
    public required int LengthGenerateCode { get; init; }
    public required int VerificationExpireMin { get; init; }
}
