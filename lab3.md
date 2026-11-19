# Wiktor Zieliński - Sprawozdanie lab3

## Tabela adresów URL z przypisanymi akcjami

| Adres | Kontroler.Akcja | Metoda HTTP |
| :--- | :--- | :--- |
| `/Book/Hello` | `Book.Hello` | Dowolna |
| `/Book/Status` | `Book.Status` | Dowolna |
| `/Book/Details/{id:int}` | `Book.Details` | GET |
| `/Book/Echo` | `Book.Echo` | Dowolna |
| `/Book/Square` | `Book.Square` | Dowolna |
| `/Book/Import` | `Book.Import` | POST |
| `/Book/Inspect` | `Book.Inspect` | Dowolna |
| `/Book/Sum` | `Book.Sum` | Dowolna |
| `/Book/UserAgent` | `Book.UserAgent` | GET |
| `/Book/Health` | `Book.Health` | GET |
| `/Book/Index` | `Book.Index` | Dowolna |
| `/Book/Create` | `Book.Create` | GET |
| `/Book/Create` | `Book.Create` | POST |
| `/Book/ByYear` | `Book.ByYear` | Dowolna |
| `/Book/ByIsbn` | `Book.ByIsbn` | Dowolna |
| `/Book/Boom` | `Book.Boom` | Dowolna |
| `/Home/Index` | `Home.Index` | Dowolna |
| `/Home/Privacy` | `Home.Privacy` | Dowolna |
| `/Home/Error` | `Home.Error` | Dowolna |
| `/Home/Docs` | `Home.Docs` | Dowolna |
| `/Home/Routes` | `Home.Routes` | Dowolna |
| `/Home/Boom` | `Home.Boom` | Dowolna |
| `/Home/HttpStatus` | `Home.HttpStatus` | Dowolna |
| `/Admin/Authors/Index` | `Admin.Authors.Index` | GET |
| `/Admin/Authors/Create` | `Admin.Authors.Create` | GET |
| `/Admin/Authors/Create` | `Admin.Authors.Create` | POST |
| `/Admin/Dashboard/Index` | `Admin.Dashboard.Index` | Dowolna |

## Odpowiedzi na pytania z instrukcji

### Część D – Przeniesienie `UseMiddleware` za `UseStaticFiles()`
**Które żądania zostaną pominięte?**  
Po przeniesieniu `UseMiddleware` za `UseStaticFiles()` pominięte zostają wszystkie żądania do istniejących plików statycznych w katalogu `wwwroot` (CSS, JS, obrazy, `favicon.ico`), ponieważ `UseStaticFiles()` obsługuje je i przerywa dalsze wykonywanie potoku (*short-circuit*).

### Część F – `/Book/Status?code=418` względem Lab 1
**Co się teraz zmieniło w odpowiedzi na Zadanie C1 z Lab 1?**  
W Lab 1 akcja `Status(418)` zwracała wynik `StatusCode(418)` z pustym ciałem odpowiedzi, przez co przeglądarka wyświetlała pustą stronę lub domyślny komunikat błędu. Po dodaniu `app.UseStatusCodePagesWithReExecute("/Home/HttpStatus", "?code={0}");` middleware przechwytuje każdą odpowiedź z kodem błędu (`400–599`), która nie posiada własnej treści i ponownie wykonuje potok dla ścieżki `/Home/HttpStatus?code=418`, generując pełną stronę HTML z layoutem aplikacji przy zachowaniu oryginalnego kodu statusu HTTP 418.

### Zadanie 4 – Kolejność wykonywania filtrów akcji
**W jakiej kolejności wykonują się dwa filtry nad tą samą akcją (`OnActionExecuting` / `OnActionExecuted`)?**  
Filtry wykonują się kaskadowo (na zasadzie stosu / LIFO). Najpierw przed samą akcją uruchamiają się metody `OnActionExecuting` od filtru najbardziej zewnętrznego do wewnętrznego (`LogActionFilter` -> `Filtr-A` -> `Filtr-B` -> `MeasureTime`), następnie wykonuje się właściwa metoda akcji (`BookController.Hello()`), a po niej uruchamiają się metody `OnActionExecuted` w odwrotnej kolejności – od filtru najbardziej wewnętrznego do zewnętrznego (`MeasureTime` -> `Filtr-B` -> `Filtr-A` -> `LogActionFilter`).

**Czy jest jakiś parametr określający kolejność?**  
Tak, kolejnością steruje właściwość `Order` (typu `int`, domyślnie `0`) z interfejsu `IOrderedFilter` (implementowanego m.in. przez `ActionFilterAttribute` i `ServiceFilterAttribute`). Filtr z mniejszą wartością `Order` uruchamia `OnActionExecuting` wcześniej, a `OnActionExecuted` później. Jeśli dwa filtry mają tę samą wartość `Order`, decyduje miejsce ich rejestracji: najpierw filtry globalne, potem na poziomie kontrolera, a na końcu na poziomie konkretnej akcji.

## Lokalizacja wybranych elementów w kodzie

* **Test kolejności dwóch filtrów akcji (Zadanie 4):** `LabApp/Filters/SecondActionFilterAttribute.cs` – nałożony dwukrotnie z różnymi wartościami `Order` nad akcją `BookController.Hello()` (do przetestowania pod adresem `/Book/Hello`).
* **Filtr `[MeasureTime]` / nagłówek `X-Action-Ms` (Zadanie 3):** `LabApp/Filters/MeasureTimeAttribute.cs` – podpięty nad kontrolerem `BookController` oraz nad akcją `BookController.Hello()`.
* **Trasa `archive/{year:int}/{month:int:range(1,12)}` (Zadanie 1):** `LabApp/Program.cs` (trasa `"books-archive"`) kierująca do istniejącej akcji `BookController.ByYear(int year)`.
* **Licznik żądań dla każdej ścieżki i Top 5 (Zadanie 2):** `LabApp/Middleware/RequestTimingMiddleware.cs` (`RequestStats.PathCounts` oparte o `ConcurrentDictionary<string, long>`) oraz prezentacja wyników w `LabApp/Areas/Admin/Controllers/DashboardController.cs` (`/Admin/Dashboard`).
* **Akcje testowe wyjątków (`Boom`):** zaimplementowane zarówno jako `HomeController.Boom()` (`/Home/Boom`), jak i `BookController.Boom()` (`/Book/Boom`).