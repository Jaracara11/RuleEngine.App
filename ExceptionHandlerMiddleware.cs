using System.Net;
using System.Text.Json;

namespace RuleEngine.App
{
    public class ExceptionHandlerMiddleware(
        RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");

                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");

                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                context.Response.StatusCode,
                ex.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}