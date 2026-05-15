using Kamirion.RepairShop.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Security.Claims;

namespace Kamirion.RepairShop.Web.Middleware;

public sealed class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IWebHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("TenantId", ResolveTenantId(context)))
            using (LogContext.PushProperty("UserId", ResolveUserId(context)))
            using (LogContext.PushProperty("RequestPath", context.Request.Path.Value))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
            {
                logger.LogError(ex, "Unhandled exception on {RequestMethod} {RequestPath}",
                    context.Request.Method, context.Request.Path);
            }

            if (context.Response.HasStarted)
                throw;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Instance = context.TraceIdentifier
            };

            if (env.IsDevelopment())
            {
                problem.Detail = ex.Message;
                problem.Extensions["stackTrace"] = ex.StackTrace;
            }

            await context.Response.WriteAsJsonAsync(problem);
        }
    }

    // TenantContext puede no estar resuelto si la excepción ocurre antes del middleware de tenancy
    private static string? ResolveTenantId(HttpContext context)
    {
        try { return context.RequestServices.GetService<TenantContext>()?.TenantId; }
        catch { return null; }
    }

    private static string? ResolveUserId(HttpContext context) =>
        context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
