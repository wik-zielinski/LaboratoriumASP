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
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(BookFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        BookStoreMemory.Books.Add(new Book()
        {
            Id = BookStoreMemory.Books.Max(x => x.Id) + 1,
            Title = model.Title,
            Author = model.Author,
            PublicationDate =  model.PublicationDate,
            Price =  model.Price,
            NumPages =  model.NumPages,
            Isbn =  model.Isbn,
        });
        TempData["Success"] =  "Book created successfully";
        return RedirectToAction(nameof(Index));
    }
}