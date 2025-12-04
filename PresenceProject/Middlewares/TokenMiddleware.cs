namespace PresenceProject.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenMiddleware> _logger;

        public TokenMiddleware(RequestDelegate next, ILogger<TokenMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;

            _logger.LogInformation("Incoming request: {method} {path}", method, path);

            await _next(context);
        }
    }

    public static class TokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
