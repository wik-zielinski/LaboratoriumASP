using System.Diagnostics;
using LabApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LabApp.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _log;
    private readonly IModelMetadataProvider _metadataProvider;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> log, IModelMetadataProvider metadataProvider)
    {
        _log = log;
        _metadataProvider = metadataProvider;
    }

    public void OnException(ExceptionContext context)
    {
        _log.LogError(context.Exception, "Nieobsłużony wyjątek w akcji {Action}", 
            context.ActionDescriptor.DisplayName);

        if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Wystąpił błąd serwera.",
                Detail = context.Exception.Message
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        else
        {
            var viewData = new ViewDataDictionary(_metadataProvider, context.ModelState)
            {
                Model = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
                }
            };

            context.Result = new ViewResult
            {
                ViewName = "Error",
                StatusCode = StatusCodes.Status500InternalServerError,
                ViewData = viewData
            };
        }

        context.ExceptionHandled = true;
    }
}
