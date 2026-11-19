# Wiktor Zieliński - Sprawozdanie lab1

## Część A – Zadanie A1 (Rola elementów projektu)

| Element | Rola |
| :--- | :--- |
| `Program.cs` | Główny punkt wejścia aplikacji, w którym uruchamiany jest start całego systemu. Konfiguruje się w nim usługi, potok żądań (*middleware*) oraz definiuje zasady routingu. |
| `Controllers/` | Folder grupujący klasy kontrolerów, które przyjmują żądania HTTP i na ich podstawie wywołują odpowiednią logikę biznesową. Ich głównym zadaniem jest sformułowanie odpowiedzi, zazwyczaj poprzez przekazanie danych modelu do odpowiedniego widoku. |
| `Models/` | Folder zawierający klasy reprezentujące dane domeny oraz modele widoków (ViewModel/DTO). Zamykana jest w nim struktura danych i reguły ich przetwarzania przed wysłaniem do widoku lub bazy. |
| `Views/` | Folder z plikami szablonów opartymi na składni silnika Razor (`.cshtml` – połączenie HTML z C#). Służą one wyłącznie do prezentacji danych użytkownikowi (`Views/Shared` zawiera layout i widoki częściowe). |
| `wwwroot/` | Folder przeznaczony na pliki statyczne, takie jak arkusze stylów CSS, skrypty JS, obrazy i biblioteki. Jest to jedyny katalog serwowany bezpośrednio do przeglądarki klienta, bez przetwarzania przez kontrolery. |
| `appsettings.json` | Plik przechowujący główną konfigurację aplikacji webowej, m.in. ustawienia logowania czy łańcuchy połączeń do bazy danych. |
| `Properties/launchSettings.json` | Plik odpowiedzialny za konfigurację profili uruchamiania aplikacji w środowisku deweloperskim. Określa m.in. porty sieciowe, na których aplikacja nasłuchuje podczas testowania i tworzenia kodu. |

## Część B – Podgląd HTTP w przeglądarce

| Pytanie | Odpowiedź |
| :--- | :--- |
| Metoda żądania | `GET` |
| Kod statusu odpowiedzi | `200 OK` |
| `Content-Type` odpowiedzi | `text/html; charset=utf-8` |
| Nagłówek `User-Agent` żądania | `Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/155.0.0.0 Safari/537.36` |
| Ile żądań wykonała strona łącznie i dlaczego więcej niż jedno? | 8 żądań. Oprócz samego dokumentu HTML przeglądarka pobiera również pliki statyczne (arkusze CSS, skrypty JS oraz ikonę `favicon.ico`). |

**Wejdź na nieistniejący adres `/abc` – jaki kod statusu widzisz?**  
`HTTP ERROR 404 – Not Found`.

## Część C – Zadanie C1 (`StatusCode`)

**Wyniki wywołań w zakładce Network:**
* `/Book/Status?code=404` – status `404 Not Found`
* `/Book/Status?code=418` – status `418 I'm a teapot`
* `/Book/Status?code=500` – status `500 Internal Server Error`

**Czy `StatusCode(404)` pokazuje stronę błędu? Dlaczego nie?**  
Nie, `StatusCode(404)` nie wyświetla strony błędu pochodzącej z naszej aplikacji. Dzieje się tak, ponieważ metoda ta zwraca jedynie sam nagłówek HTTP ze statusem `404`, bez jakiejkolwiek treści (pusty *body*).

## Część D – Sprawdzenie w Postmanie

* **`POST /Book/Import` z `Content-Type: application/json`:** zwraca status `200 OK` oraz odesłane pola w formacie JSON.
* **`POST /Book/Import` z `Content-Type: text/plain`:** zwraca błąd `415 Unsupported Media Type`, ponieważ powiązanie `[FromBody]` wymaga zgodnego nagłówka `Content-Type`.
* **`GET /Book/Import`:** zwraca kod `404 Not Found` zamiast `405 Method Not Allowed`. Dzieje się tak, ponieważ przy ustawionym routingu konwencjonalnym framework po odrzuceniu akcji przez atrybut `[HttpPost]` nie znajduje innej pasującej akcji i uznaje trasę za nieistniejącą.

## Zadania do samodzielnego wykonania

### Zadanie 1 – Akcja `Sum(int a, int b)`
**Co się stanie po `?a=x`? Jaki jest kod statusu i dlaczego?**  
Po wpisaniu `/Book/Sum?a=x&b=3` otrzymujemy wynik `3`, a kod statusu to `200 OK`. Mechanizm wiązania modeli (*model binding*) próbuje przekonwertować wartość `"x"` z paska adresu na typ `int`. W ASP.NET Core nieudana konwersja nie przerywa działania aplikacji – zamiast tego parametrowi `a` przypisywana jest wartość domyślna dla typu `int`, czyli `0`. W efekcie równanie przyjmuje postać `0 + 3 = 3`, a serwer zwraca poprawną odpowiedź `200 OK`.

### Zadanie 2 – Nagłówek `User-Agent`
* **Przeglądarka:** `Twój User-Agent to: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/155.0.0.0 Safari/537.36`
* **Postman:** `Twój User-Agent to: PostmanRuntime/2.11.2`

**Wniosek:** Różni klienci HTTP (przeglądarka vs Postman) wysyłają do serwera różne nagłówki `User-Agent`, dzięki czemu aplikacja serwerowa może rozpoznać, jaki klient się z nią komunikuje.

### Zadanie 3 – Akcja `/Book/Health`
Zwraca JSON w formacie `{"status":"ok","time":"..."}` ze statusem `200 OK` oraz nagłówkiem odpowiedzi `Content-Type: application/json; charset=utf-8`. Użycie wbudowanej metody `Json()` w ASP.NET Core automatycznie ustawia właściwy nagłówek `Content-Type` bez konieczności robienia tego ręcznie.

## Zrzuty ekranu (Efekt do zaliczenia)

*(Jeśli umieszczasz zrzuty w repozytorium, zapisz je np. w folderze `screenshots/` i podmień nazwy plików poniżej)*
* Zakładka Network dla `/Book/Echo?name=Ala`: `![Network Echo](screenshots/echo-network.png)`
* Postman – poprawny `POST /Book/Import` (`200 OK`): `![Postman 200](screenshots/postman-import-200.png)`
* Postman – błąd `415 Unsupported Media Type`: `![Postman 415](screenshots/postman-import-415.png)`

## Lokalizacja wybranych elementów w kodzie

* **Kontroler z akcjami `Hello`, `Status`, `Details`, `Echo`, `Square`, `Import`, `Inspect`, `Sum`, `UserAgent`, `Health`:** `LabApp/Controllers/BookController.cs`.
* **Klasa DTO dla `[FromBody]`:** `LabApp/Models/BookDto.cs`.
* **Widok szczegółów książki:** `LabApp/Views/Book/Details.cshtml`.