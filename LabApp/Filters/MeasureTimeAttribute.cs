using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LabApp.Filters;

public class MeasureTimeAttribute : ActionFilterAttribute
{
    private readonly ILogger<MeasureTimeAttribute>? _logger;

    public MeasureTimeAttribute() { }

    public MeasureTimeAttribute(ILogger<MeasureTimeAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["ActionStopwatch"] = Stopwatch.StartNew();
        _logger?.LogInformation("START MeasureTime (Order={Order}) dla {Action}", 
            Order, context.ActionDescriptor.DisplayName);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items["ActionStopwatch"] is Stopwatch sw)
        {
            sw.Stop();
            context.HttpContext.Response.Headers["X-Action-Ms"] = sw.ElapsedMilliseconds.ToString();
            _logger?.LogInformation("KONIEC MeasureTime (Order={Order}) dla {Action}: {Ms} ms", 
                Order, context.ActionDescriptor.DisplayName, sw.ElapsedMilliseconds);
        }
    }
}