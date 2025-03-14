namespace Cafe_App.CustomMiddlewares;

public class CustomThemeMiddleware
{
    private readonly RequestDelegate _next;

    public CustomThemeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var themeFromCookie = context.Request.Cookies["theme"];
        if (themeFromCookie != null)
        {
            context.Items["theme"] = themeFromCookie;
        }

        await _next.Invoke(context);
    }
}