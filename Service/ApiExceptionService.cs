using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models;

namespace Services
{
    public class ApiExceptionService : IApiExceptionService
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiExceptionService(ILogger<ApiExceptionService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public ArgumentException Create(HttpStatusCode code, string message)
        {
            var exception = new ArgumentException($"{(int)code} {code} {message}");
            _logger.LogError(exception, message);
            _httpContextAccessor.HttpContext.Response.SetHttpStatusCode(code);
            return exception;
        }

        public ArgumentException Create(Exception exception, HttpStatusCode code, string message)
        {
            var newMessage = $"{(int)code} {code} {message}";
            _logger.LogError(exception, newMessage);
            _httpContextAccessor.HttpContext.Response.SetHttpStatusCode(code);
            return new ArgumentException( newMessage, exception);
        }
    }

    public interface IApiExceptionService
    {
        ArgumentException Create(HttpStatusCode code, string message);
        ArgumentException Create(Exception exception, HttpStatusCode code, string message);

    }
}
