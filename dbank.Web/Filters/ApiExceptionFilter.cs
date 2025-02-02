using System.Text.Json;
using DBank.Domain.Exceptions;
using DBank.Domain.Extensions;
using DBank.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DBank.Web.Filters;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        ApiErrorResponse response;

        switch (true)
        {
            case { } when exception is EntityNotFoundException:
            {
                response = new ApiErrorResponse
                {
                    StatusCode = 404,
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }
            case { } when exception is DuplicateEntityException:
            {
                response = new ApiErrorResponse()
                {
                    StatusCode = 409,
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }
            default:
            {
                response = new ApiErrorResponse
                {
                    StatusCode = 500,
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }
        }

        logger.LogError($"Api method {context.HttpContext.Request.Path} completed with code {response.StatusCode} and error " +
                        $"{JsonSerializer.Serialize(response)}");

        context.Result = new JsonResult(new {response}){StatusCode = response.StatusCode};
    }
}
