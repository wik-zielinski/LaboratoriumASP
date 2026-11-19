using System.Diagnostics;

namespace LabApp.Middleware;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _log;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> log)
    { _next = next; _log = log; }

    public async Task InvokeAsync(HttpContext ctx, RequestStats stats)
    {
        var sw = Stopwatch.StartNew();
        var path = ctx.Request.Path.Value ?? "/";
        var n = stats.Hit(path);
        // nagłówki z licznikiem i czasem dodajem tuż przed odesłaniem odpowiedzi
        ctx.Response.OnStarting(() =>                     {
            ctx.Response.Headers["X-Elapsed-Ms"] = sw.ElapsedMilliseconds.ToString();
            ctx.Response.Headers["X-Request-Count"] = n.ToString();
            return Task.CompletedTask;
        });
        if (ctx.Request.Path.StartsWithSegments("/blokada")) { ctx.Response.StatusCode = 403; return; }
        // wywołanie następnej warstwy potoku
        await _next(ctx);                                 

        _log.LogInformation("{Method} {Path} -> {Status} w {Ms} ms", ctx.Request.Method,
            ctx.Request.Path, ctx.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}