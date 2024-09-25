namespace Forum.API.Middleware;

public class AccessControllerMiddleware
{
    private readonly RequestDelegate _next;

    public AccessControllerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);
    }
}
