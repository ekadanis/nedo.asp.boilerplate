using System.Globalization;
using Nedo.AspNet.Request.Validation.Abstractions;

namespace Nedo.Asp.Boilerplate.API.Middleware;

public sealed class RequestCultureMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IRequestValidationContext validationContext)
    {
        var culture = TryGetFromHeader(context);

        if (culture is not null)
        {
            validationContext.CurrentCulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        await _next(context);
    }

    private static CultureInfo? TryGetFromHeader(HttpContext context)
    {
        var header = context.Request.Headers["Accept-Language"]
            .FirstOrDefault()
            ?.Split(',')[0]
            .Split('-')[0]
            .Trim();

        return TryCreateCulture(header);
    }

    private static CultureInfo? TryGetFromJwt(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated != true)
            return null;

        return TryCreateCulture(context.User.FindFirst("language")?.Value);
    }

    private static CultureInfo? TryCreateCulture(string? cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName))
            return null;

        try
        {
            return new CultureInfo(cultureName);
        }
        catch (CultureNotFoundException)
        {
            return null;
        }
    }
}
