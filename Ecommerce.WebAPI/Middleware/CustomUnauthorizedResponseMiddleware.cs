using System.Text.Json;

namespace BookStoreClean2.Middleware;

public class CustomUnauthorizedResponseMiddleware
{

    private readonly RequestDelegate _next;

    public CustomUnauthorizedResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            // Add your custom unauthorized handling logic here
        }
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized || context.Response.StatusCode == StatusCodes.Status404NotFound)

{
            var response = new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.2",
                title = "unatuhorized",
                status = context.Response.StatusCode,
                traceId = context.TraceIdentifier,
            };
            
            
            var responseJson = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseJson);
        }
    }
}