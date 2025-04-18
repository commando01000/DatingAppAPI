using Common.Layer;
using System.Net;
using System.Text.Json;

namespace DatingAppAPI.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment hostEnvironment;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = hostEnvironment.IsDevelopment() ? new Response<Nothing>()
                {
                    StatusCode = context.Response.StatusCode,
                    Status = false,
                    Message = ex.Message,
                } : new Response<Nothing>() { StatusCode = context.Response.StatusCode, Status = false, Message = "Internal Server Error." };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper
                };

                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            };
        }
    }
}
