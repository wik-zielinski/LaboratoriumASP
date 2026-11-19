using Microsoft.AspNetCore.Mvc.Filters;

namespace LabApp.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class SecondActionFilterAttribute : ActionFilterAttribute
{
    private readonly string _name;

    public SecondActionFilterAttribute(string name)
    {
        _name = name;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<SecondActionFilterAttribute>>();
        logger.LogInformation(">>> [{Name} | Order={Order}] OnActionExecuting", _name, Order);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<SecondActionFilterAttribute>>();
        logger.LogInformation("<<< [{Name} | Order={Order}] OnActionExecuted", _name, Order);
    }
}