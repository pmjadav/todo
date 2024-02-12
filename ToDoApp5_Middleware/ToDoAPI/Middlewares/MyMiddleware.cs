using System.Security.Permissions;

namespace ToDoAPI.Middlewares
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MyMiddleware> _logger;
        public MyMiddleware(RequestDelegate next, ILogger<MyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //_logger.LogInformation(DateTime.Now.ToString());
            _logger.LogInformation("My Middleware Executing!!!");
            await _next(context);
        }
    }
}
