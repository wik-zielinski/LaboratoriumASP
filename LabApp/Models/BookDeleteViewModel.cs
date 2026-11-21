using Microsoft.AspNetCore.Mvc;

namespace LabApp.Models;

public class BookDeleteViewModel
{
    [HiddenInput]
    public int Id { get; set; }
}