using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Student_minimal_api.Middleware
{
    public class ReqLogMiddleware_2
    {
        private readonly RequestDelegate _next;

        public ReqLogMiddleware_2(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Логируем начало обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} начал обрабатываться");
            Console.WriteLine($"ReqLogMiddleware_2 Запрос {context.Request.Method} {context.Request.Path} начал обрабатываться");
            // Передаем управление следующему компоненту в цепочке
            await _next(context);

            // Логируем завершение обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} обработан с статус-кодом {context.Response.StatusCode}");

            Console.WriteLine($"ReqLogMiddleware_2 Запрос {context.Request.Method} {context.Request.Path} обработан ");
        }
    }

    public static class ReqLogMiddleware_2Extensions
    {
        public static IApplicationBuilder UseRequestLogging_2(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ReqLogMiddleware_2>();
        }
    }

    public class PostRequestLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "POST")
            {
                // Здесь вы можете записать информацию о POST запросе, например, в лог файл или базу данных
                Console.WriteLine($"POST request: {context.Request.Path} at {DateTime.Now}");
            }
            await next(context);
        }
    }
}
