using System.Net;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class ApiExceptionService : IApiExceptionService
    {
        private readonly ILogger _logger;

        public ApiExceptionService(ILogger<ApiExceptionService> logger)
        {
            _logger = logger;
        }

        public Exception Create(HttpStatusCode code, string message)
        {
            var exception = new Exception($"{(int)code} {code} {message}");
            _logger.LogError(exception, message);
            return exception;
        }

        public Exception Create(Exception exception, HttpStatusCode code, string message)
        {
            var newMessage = $"{(int)code} {code} {message}";
            _logger.LogError(exception, newMessage);
            return new Exception( newMessage, exception);
        }
    }

    public interface IApiExceptionService
    {
        Exception Create(HttpStatusCode code, string message);
        Exception Create(Exception exception, HttpStatusCode code, string message);

    }
}
