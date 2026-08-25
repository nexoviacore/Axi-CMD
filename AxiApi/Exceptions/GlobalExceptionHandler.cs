using AxiApi.DTOs;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using Npgsql; 


namespace AxiApi.Exceptions
{
    public class GlobalExceptionHandler: IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger; 

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger; 
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occured");

            HttpStatusCode statusCode;
            string message; 

            switch (exception)
            {
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
               

                case KeyNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

              
                case DatabaseException _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case RedisCacheConnectionException _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occured";
                    break; 
            }

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            var response = new ApiResponseDTO
            {
                Success = false,
                Message = message,
                StatusCode = httpContext.Response.StatusCode

            };

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response), cancellationToken);
            return true; 
        }
    }
}
