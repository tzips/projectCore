using System.Net;

namespace PresenceProject.Middlewares
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
                _logger.LogInformation("Handling request: {method} {url} by {user}",
                    context.Request.Method,
                    context.Request.Path,
                    context.User?.Identity?.Name ?? "Anonymous");

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred while processing request: {method} {url}",
                    context.Request.Method, context.Request.Path);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("אירעה שגיאה בלתי צפויה בשרת.");
            }
        }
    }
}
