using LabApp.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly RequestStats _stats;

    public DashboardController(RequestStats stats)
    {
        _stats = stats;
    }

    public IActionResult Index()
    {
        ViewBag.TotalRequests = _stats.Total;
        var topPaths = _stats.GetTopPaths(5);
        return View(topPaths);
    }
}