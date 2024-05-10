using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Student_minimal_api.Middleware
{
    public class ReqLogMiddleware_3
    {
        private readonly RequestDelegate _next;

        public ReqLogMiddleware_3(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Логируем начало обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} начал обрабатываться");
            Console.WriteLine($"ReqLogMiddleware_3 Запрос {context.Request.Method} {context.Request.Path} начал обрабатываться");
            // Передаем управление следующему компоненту в цепочке
            await _next(context);

            // Логируем завершение обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} обработан с статус-кодом {context.Response.StatusCode}");

            Console.WriteLine($"ReqLogMiddleware_3 Запрос {context.Request.Method} {context.Request.Path} обработан ");
        }
    }

    public static class ReqLogMiddleware_3Extensions
    {
        public static IApplicationBuilder UseRequestLogging_3(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ReqLogMiddleware_3>();
        }
    }

    public class RequestLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Здесь вы можете записать информацию о любом запросе, например, в лог файл или базу данных
            Console.WriteLine($"{context.Request.Method} request: {context.Request.Path} at {DateTime.Now}");
            await next(context);
        }
    }

}
