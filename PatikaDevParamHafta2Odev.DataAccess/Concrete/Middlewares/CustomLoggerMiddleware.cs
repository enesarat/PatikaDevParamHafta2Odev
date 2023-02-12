using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PatikaDevParamHafta2Odev.DataAccess.Concrete.Middlewares
{ 
    public class CustomLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomLoggerMiddleware> _logger;
        public CustomLoggerMiddleware(RequestDelegate next, ILogger<CustomLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string controllerName = context.Request.RouteValues["controller"].ToString();
            string actionName = context.Request.RouteValues["action"].ToString();

            _logger.LogInformation($"{actionName} endpoint was started.");
            await _next.Invoke(context);
            _logger.LogInformation($"{actionName} endpoint was ended.");
        }
    }
}
