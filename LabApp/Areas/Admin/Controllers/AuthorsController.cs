using LabApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthorsController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(BookStoreMemory.Authors);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Author());
    }

    [HttpPost]
    public IActionResult Create(Author model)
    {
        if (!ModelState.IsValid) return View(model);
        model.Id = BookStoreMemory.Authors.Any() ? BookStoreMemory.Authors.Max(a => a.Id) + 1 : 1;
        
        BookStoreMemory.Authors.Add(model);
        return RedirectToAction(nameof(Index));
    }
}