using System.Net.Mime;
using System.Text.Json;

namespace MovieApi
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment environment;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            this.logger = logger;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message,ex.InnerException.Message);
                await HandleExceptionAsync(context, ex);    

            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = environment.IsDevelopment()
                ? new CustomExceptionResponse(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString(), ex.InnerException.Message)
                : new CustomExceptionResponse(context.Response.StatusCode,"Internal Server Error");

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
