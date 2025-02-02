namespace DBank.Domain.Options;

public class JwtOptions
{
    public required string TokenPrivateKey { get; init; }
    public required string TokenAudience { get; init; }
    public required string TokenIssuer { get; init; }
    public required int ExpireHours { get; init; }
}
