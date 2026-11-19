using System.ComponentModel.DataAnnotations;

namespace LabApp.Models;

public class Author
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Imię i nazwisko autora jest wymagane")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nazwa autora: 2-100 znaków")]
    [Display(Name = "Imię i nazwisko")]
    public string Name { get; set; } = "";
}