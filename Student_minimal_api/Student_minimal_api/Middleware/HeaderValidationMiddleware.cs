﻿namespace Student_minimal_api.Middleware
{

    public class HeaderValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HeaderValidationMiddleware> _logger;

        public HeaderValidationMiddleware(RequestDelegate next, ILogger<HeaderValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Проверка наличия заголовка и его значения
            if (context.Request.Headers.TryGetValue("X-Special-Header", out var headerValue))
            {
                if (headerValue == "ValidValue")
                {
                    _logger.LogInformation("Access granted: X-Special-Header is valid.");
                    await _next(context); // Заголовок присутствует и значение корректно, продолжаем обработку
                }
                else
                {
                    // Заголовок есть, но его значение некорректно
                    _logger.LogWarning($"Access denied: X-Special-Header value '{headerValue}' is invalid. Request for {context.Request.Path} was denied.");
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Invalid header value");
                }
            }
            else
            {
                // Заголовок отсутствует
                _logger.LogWarning($"Access denied: Missing X-Special-Header. Request for {context.Request.Path} was denied.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Missing header");

            }
        }


        //    public async Task InvokeAsync(HttpContext context)
        //    {
        //        if (!context.Request.Headers.ContainsKey("X-Special-Header"))
        //        {
        //            _logger.LogWarning($"Access denied: Missing X-Special-Header. Request for {context.Request.Path} was denied.");
        //            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        //            return;
        //        }

        //        if (context.Request.Headers.ContainsKey("X1-Special-Header"))
        //        {
        //            _logger.LogWarning($"X1-Special-Header");

        //        }

        //        if (context.Request.Headers.ContainsKey("X2-Special-Header"))
        //        {
        //            _logger.LogWarning($"X2-Special-Header");

        //        }

        //        _logger.LogInformation("Access granted: X-Special-Header found.");
        //        await _next(context);
        //    }
        //}

        //public static class HeaderValidationMiddlewareExtensions
        //{
        //    public static IApplicationBuilder UseHeaderValidation(this IApplicationBuilder builder)
        //    {
        //        return builder.UseMiddleware<HeaderValidationMiddleware>();
        //    }
        //}
    }
}
