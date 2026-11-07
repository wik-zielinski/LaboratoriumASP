using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LabApp.Models;

public class RouteInfo
{
    public string Pattern { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public string? Area { get; set; }
    public string Methods { get; set; }
}