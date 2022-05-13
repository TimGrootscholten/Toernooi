using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models;

namespace Services;

public class ApiExceptionService : IApiExceptionService
{
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiExceptionService(ILogger<ApiExceptionService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public ApiException Create(HttpStatusCode code, string message)
    {
        var exception = new ApiException(message);
        _logger.LogError(exception, message);
        _httpContextAccessor.HttpContext.Response.SetHttpStatusCode(code);
        return exception;
    }
}

public interface IApiExceptionService
{
    ApiException Create(HttpStatusCode code, string message);
}