using LabApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Controllers;

public class BookController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View(BookStoreMemory.Books);
    }
}