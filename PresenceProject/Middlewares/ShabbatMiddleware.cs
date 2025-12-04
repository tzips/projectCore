namespace PresenceProject.Middlewares
{
    public class ShabbatMiddleware
    {
        private readonly RequestDelegate _next;
        public ShabbatMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)

        {
            var Shabat = false;
            DayOfWeek today = DateTime.Now.DayOfWeek;
            if (today == DayOfWeek.Saturday)
            {
                Shabat = true;
            }
            if (Shabat)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest; return;
            }
            await _next(context);

        }
    }
}
