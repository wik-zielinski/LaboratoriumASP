using System.Text;
using LabApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Controllers;

public class BookController : Controller
{
// (0) GET /Book/Hello - zwykły tekst
    public IActionResult Hello() => Content("Witaj w BookStore!", "text/plain; charset=utf-8");
    
    // GET /Book/Status?code=418 - dowolny kod statusu
    public IActionResult Status(int code = 200) => StatusCode(code);
    
    // GET /Book/Details/3 - widok
    public IActionResult Details(int id)
    {
        ViewBag.Id = id;
        return View();
    }
    
    // (1) query string:  /Book/Echo?name=Ala&count=3
    public IActionResult Echo([FromQuery] string name = "nieznajomy", [FromQuery] int count = 1) =>
        Content(string.Join("\n", Enumerable.Repeat($"Cześć, {name}!", Math.Clamp(count, 1, 20))));

    // (2) zmienna ścieżki:  /Book/Square/7   (segment {id} z domyślnej trasy)
    public IActionResult Square(int id) => Content($"{id}^2 = {id * id}");
    
    // (3) ciało żądania JSON:  POST /Book/Import
    [HttpPost]
    public IActionResult Import([FromBody] BookDto dto) =>
        Json(new { received = dto.Title, dto.Isbn, dto.Price });

    // (4) surowy obiekt Request
    public IActionResult Inspect()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Method: {Request.Method}");
        sb.AppendLine($"Path:   {Request.Path}{Request.QueryString}");
        foreach (var h in Request.Headers) sb.AppendLine($"{h.Key}: {h.Value}");
        return Content(sb.ToString(), "text/plain; charset=utf-8");
    }
    
    // (5) Akcja Sum(int a, int b)
    public IActionResult Sum(int a, int b)
    {
        int result = a + b;
        return Content(result.ToString());
    }
    
    // (6) Akcja zwracająca nagłówek User-Agent
    [HttpGet]
    public IActionResult UserAgent()
    {
        var userAgent = Request.Headers.UserAgent.ToString();
        return Content ($"Twój User-Agent to: {userAgent}");
    }
    
    // (7) Akcja zwracająca status ok i aktualną date i czas
    [HttpGet]
    public IActionResult Health()
    {
        return Json(new { status = "ok", time = DateTime.Now });
    }
    
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
