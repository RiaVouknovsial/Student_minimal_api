namespace Student_minimal_api.Interface
{
    public interface IMiddleware
    {
        Task InvokeAsync(HttpContext context, RequestDelegate next);
    }
}
