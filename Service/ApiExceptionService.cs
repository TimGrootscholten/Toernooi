using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Models;

namespace Services
{
    public class ApiExceptionService : IApiExceptionService
    {
        private readonly ILogger _logger;

        public ApiExceptionService(ILogger<ApiExceptionService> logger)
        {
            _logger = logger;
        }

        public ApiException Create(HttpStatusCode code, string message)
        {
            return new ApiException(code, message);
        }

        public ApiException Create(Exception exception, HttpStatusCode code, Enums.MessageText errorcode)
        {
            _logger.LogError(exception, errorcode.GetDescription());
            return new ApiException(code, errorcode.GetDescription());
        }
    }

    public interface IApiExceptionService
    {
        ApiException Create(HttpStatusCode code, string message);
    }
}
