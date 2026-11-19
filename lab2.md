# Wiktor Zieliński - Sprawozdanie lab2

## Odpowiedzi na pytania z instrukcji

### Część B – Zadanie B1 (Sekcje w układzie strony)
**Co się stanie, gdy widok zdefiniuje sekcję, której layout nie renderuje?**  
Gdy widok zdefiniuje sekcję (np. `@section NieIstnieje { ... }`), której plik `_Layout.cshtml` w ogóle nie wywołuje (ani przez `RenderSection`, ani `RenderSectionAsync`), ASP.NET Core rzuca w trakcie działania wyjątek `InvalidOperationException` z komunikatem informującym, że zdefiniowane sekcje nie zostały wyrenderowane przez stronę układu (`The following sections have been defined but have not been rendered by the page at '/Views/Shared/_Layout.cshtml'`).

### Część F – Sprawdzenie (pkt 3): wzorzec PRG a `return View("Index", ...)`
**Ile razy książka została dodana po odświeżeniu (F5)? Co się dzieje po zmianie na `return View("Index", ...)`?**  
Przy zastosowaniu `return RedirectToAction(nameof(Index))` po wysłaniu poprawnego formularza i odświeżeniu strony klawiszem `F5` książka zostaje dodana tylko jeden raz. Serwer po przetworzeniu żądania `POST` odpowiada kodem `302 Found` (przekierowanie), a przeglądarka wykonuje osobne żądanie `GET /Book`. Odświeżenie (`F5`) powtarza wtedy jedynie ostatnie żądanie `GET`.

Po zmianie w metodzie `POST` na `return View("Index", BookStoreMemory.Books);` i wciśnięciu `F5` przeglądarka wyświetla ostrzeżenie o ponownym przesłaniu formularza (*Confirm Form Resubmission*). Po zatwierdzeniu wysyłane jest kolejne żądanie `POST` z tymi samymi danymi, przez co ta sama książka dodaje się do listy wielokrotnie (za każdym odświeżeniem).

### Część F – Sprawdzenie (pkt 4): atrybuty `data-val-*` w HTML
**Znajdź atrybuty `data-val-*` w polu `Title` i wyjaśnij ich rolę.**  
Dla pola `Title` tag helpery generują m.in. następujące atrybuty:
* `data-val="true"` – włącza nieinwazyjną walidację po stronie klienta (*unobtrusive validation*) dla danego pola.
* `data-val-required="Tytuł jest wymagany"` – określa komunikat błędu wyświetlany, gdy pole jest puste.
* `data-val-length="Tytuł: 2–200 znaków"` – komunikat błędu dla niepoprawnej długości tekstu.
* `data-val-length-min="2"` oraz `data-val-length-max="200"` – parametry przekazywane do biblioteki `jquery.validate.unobtrusive.js`, dzięki którym skrypt wie, jakich reguł pilnować bez konieczności pisania własnego kodu JavaScript w widoku.

### Zadanie 2 – Walidacja na poziomie modelu (`IValidatableObject`)
**Gdzie wyświetlić komunikat z `IValidatableObject` (`asp-validation-summary`)?**  
Komunikat z walidacji na poziomie modelu (nieprzypisany do konkretnej właściwości) należy wyświetlić w ogólnym podsumowaniu błędów na samej górze formularza, nad polami do wprowadzania danych, za pomocą elementu: `<div asp-validation-summary="ModelOnly" class="text-danger"></div>`.

Użycie wartości `ModelOnly` sprawia, że w tym kontenerze renderują się wyłącznie błędy ogólne dotyczące całego modelu. Dzięki temu unikamy dublowania komunikatów – błędy przypisane do konkretnych pól (np. brak tytułu) pozostają wyświetlone bezpośrednio pod swoimi inputami (za co odpowiada `asp-validation-for`).

## Lokalizacja wybranych elementów w kodzie

* **Model i magazyn w pamięci (Część A):** `LabApp/Models/Book.cs` oraz `LabApp/Models/BookStoreMemory.cs`.
* **Layout z sekcjami `Styles`, `Footer`, `Scripts` i obsługą `TempData["Success"]` (Część B i Zadanie 3):** `LabApp/Views/Shared/_Layout.cshtml` oraz definicje sekcji w `LabApp/Views/Book/Index.cshtml`.
* **Lista książek i widok częściowy wiersza (Część C):** `LabApp/Views/Book/Index.cshtml` oraz `LabApp/Views/Book/_BookRow.cshtml`.
* **Model widoku, kod wydawcy (`[RegularExpression]`) i `IValidatableObject` (Część D, Zadania 1 i 2):** `LabApp/Models/BookFormViewModel.cs`.
* **Własne atrybuty walidacji `[NotInFuture]` oraz `[Isbn]` (Część E i Zadanie E1):** `LabApp/Validation/CustomValidation.cs`.
* **Formularz dodawania książki (Część F):** `LabApp/Controllers/BookController.cs` (akcje `Create`) oraz `LabApp/Views/Book/Create.cshtml`.