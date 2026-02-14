using MediCoreX.Api.Exceptions;
using MediCoreX.Api.Models;
using System.Net;
using System.Text.Json;

namespace MediCoreX.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 👉 Next middleware / controller call
                await _next(context);
            }
            catch (Exception ex)
            {
                // 🔴 Log full exception
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (ex)
            {
                case BadRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = ex.Message;
                    break;

                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = ex.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong";
                    break;
            }

            // 🔒 Stack trace sirf Development me
            response.Details = _env.IsDevelopment()
                ? ex.StackTrace
                : null;

            context.Response.StatusCode = response.StatusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
