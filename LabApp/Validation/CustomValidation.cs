using System.ComponentModel.DataAnnotations;

namespace LabApp.Models;

public class NotInFutureAttribute : ValidationAttribute
{
    public NotInFutureAttribute() => ErrorMessage = "Data nie może być z przyszłości";
    
    public override bool IsValid(object? value) => value is not DateTime d || d.Date <= DateTime.Today;
}

public class IsbnAttribute : ValidationAttribute
{
    public bool _requireCheckSum { get; set; }

    public IsbnAttribute(bool requireCheckSum = false) => _requireCheckSum = requireCheckSum;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || (value is string s && string.IsNullOrWhiteSpace(s))) 
            return ValidationResult.Success;
            
        if (value is not string isbn) 
            return new ValidationResult("ISBN musi być łańcuchem znaków.");

        string cleanedIsbn = isbn.Replace("-", "").Replace(" ", "").ToUpperInvariant();

        if (cleanedIsbn.Length == 10)
        {
            if (!cleanedIsbn[..9].All(char.IsDigit) || !(char.IsDigit(cleanedIsbn[9]) || cleanedIsbn[9] == 'X'))
                return new ValidationResult("ISBN-10 może zawierać tylko cyfry (i opcjonalnie 'X' na końcu).");

            return !_requireCheckSum || IsValidIsbn10(cleanedIsbn) 
                ? ValidationResult.Success 
                : new ValidationResult("Niepoprawna suma kontrolna ISBN-10.");
        }

        if (cleanedIsbn.Length == 13)
        {
            if (!cleanedIsbn.All(char.IsDigit)) 
                return new ValidationResult("ISBN-13 może składać się wyłącznie z cyfr.");

            return !_requireCheckSum || IsValidIsbn13(cleanedIsbn) 
                ? ValidationResult.Success 
                : new ValidationResult("Niepoprawna suma kontrolna ISBN-13.");
        }

        return new ValidationResult("ISBN musi zawierać dokładnie 10 lub 13 cyfr.");
    }

    private static bool IsValidIsbn13(string d) =>
        d.Select((c, i) => (c - '0') * (i % 2 == 0 ? 1 : 3)).Sum() % 10 == 0;

    private static bool IsValidIsbn10(string d) =>
        (d[..9].Select((c, i) => (c - '0') * (10 - i)).Sum() + (d[9] == 'X' ? 10 : d[9] - '0')) % 11 == 0;
}