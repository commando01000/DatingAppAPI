using Common.Layer;
using System.Net;
using System.Text.Json;

namespace DatingAppAPI.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (IsApiRequest(context))
                {
                    context.Response.ContentType = "application/json";

                    var response = new Response<Nothing>
                    {
                        StatusCode = context.Response.StatusCode,
                        Status = false,
                        Message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred."
                    };

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    var json = JsonSerializer.Serialize(response, options);
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    var errorMessage = _env.IsDevelopment() ? WebUtility.UrlEncode(ex.Message) : null;
                    var redirectUrl = string.IsNullOrEmpty(errorMessage)
                        ? "/Home/Error"
                        : $"/Home/Error?message={errorMessage}";

                    context.Response.Redirect(redirectUrl);
                }
            }
        }

        private bool IsApiRequest(HttpContext context)
        {
            var path = context.Request.Path.ToString();
            var accept = context.Request.Headers["Accept"].ToString();

            return path.StartsWith("/api", StringComparison.OrdinalIgnoreCase) ||
                   accept.Contains("application/json", StringComparison.OrdinalIgnoreCase);
        }
    }
}
