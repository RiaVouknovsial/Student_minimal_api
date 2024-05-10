using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Student_minimal_api.Middleware
{
    public class ReqLogMiddleware_1
    {
        private readonly RequestDelegate _next;

        public ReqLogMiddleware_1(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Логируем начало обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} начал обрабатываться");
            Console.WriteLine($"ReqLogMiddleware_1 Запрос {context.Request.Method} {context.Request.Path} начал обрабатываться");
            // Передаем управление следующему компоненту в цепочке
            await _next(context);

            // Логируем завершение обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} обработан с статус-кодом {context.Response.StatusCode}");

            Console.WriteLine($"ReqLogMiddleware_1 Запрос {context.Request.Method} {context.Request.Path} обработан ");
        }
    }

    public static class ReqLogMiddleware_1Extensions
    {
        public static IApplicationBuilder UseRequestLogging_1(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ReqLogMiddleware_1>();
        }
    }

    public class GetRequestLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "GET")
            {
                // Здесь вы можете записать информацию о GET запросе, например, в лог файл или базу данных
                Console.WriteLine($"GET request: {context.Request.Path} at {DateTime.Now}");
            }
            await next(context);
        }
    }
}
