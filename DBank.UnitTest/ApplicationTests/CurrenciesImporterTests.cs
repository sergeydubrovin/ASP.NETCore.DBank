using System.Net;
using System.Text.Json;
using DBank.Application.Models.Currencies;
using DBank.Application.Services;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;
using Moq;
using UnitTestApplication.ApplicationTestsExtensions;

namespace UnitTestApplication.ApplicationTests;

public class CurrenciesImporterTests
{
    [Fact]
    private async Task Import_SuccessfulResponse_ReturnsValuteDto()
    {
        // Arrange
        var expectedValute = Extensions.CreateExpectedValuteDto();
            
        var responseContent = JsonSerializer.Serialize(new CbCurrencyResponse { Valute = expectedValute });
        
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.GetMockHandler(HttpStatusCode.OK, responseContent);
        
        var mockCbOptions = new Mock<IOptions<CbOptions>>();
        var cbOptions = new CbOptions { BaseUrl = "https://example.ru", CurrencyPath = "currency" };
        mockCbOptions.Setup(x => x.Value).Returns(cbOptions);

        using var httpClient = new HttpClient(mockHandler.Object);
        httpClient.BaseAddress = new Uri("https://example.ru");
        
        var mockClientFactory = new Mock<IHttpClientFactory>();
        mockClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        
        var currencyService = new CurrenciesImporter(mockCbOptions.Object, mockClientFactory.Object);
        
        // Act
        var result = await currencyService.Import();
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ValuteDto>(result);
        
        Assert.Equivalent(expectedValute, result);
        
        mockHandler.Verify();
    }
}
