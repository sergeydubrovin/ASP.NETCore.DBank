namespace DBank.Web.Models;

public class ApiErrorResponse
{
    public required int StatusCode { get; set; }
    public required string Message { get; set; }
    public string? Description { get; set; }
}
