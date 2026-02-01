using System.Text.Json;
using Personal.Finance.Api.Exceptions;
using Personal.Finance.Api.Responses;

namespace Personal.Finance.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) 
        { 
            _next = next; 
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex) 
            {
                _logger.LogWarning(
                    ex,
                    "Handled application exxception. Code: {Code}, Path: {Path}",
                    ex.ErrorCode,
                    context.Request.Path
                    );

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.Fail(ex.ErrorCode, ex.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)  
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception. Path: {Path}",
                    context.Request.Path
                    );

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.Fail(
                    "INTERNAL_SERVER_ERROR",
                    "Internal server error"
                    );

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                //var message = ex.InnerException?.Message ?? ex.Message;

                //context.Response.StatusCode = 500;
                //await context.Response.WriteAsJsonAsync(new
                //{
                //    error = message
                //});
            }
        }
    }
}
