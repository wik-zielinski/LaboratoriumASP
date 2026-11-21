using Microsoft.EntityFrameworkCore;

namespace LabApp.Models.Ef;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        //base.OnModelCreating(modelBuilder);
        mb.Entity<Book>().HasData(
            new() { Id = 1, Title = "Solaris",   Isbn = "978-83-08-06534-1", PublicationDate = new(1961, 1, 1), NumPages = 204, Price = 39.90m, Author = "Stanisław Lem" },
            new() { Id = 2, Title = "Cyberiada", Isbn = "978-83-08-06535-8", PublicationDate = new(1965, 1, 1), NumPages = 296, Price = 44.00m, Author = "Stanisław Lem" },
            new() { Id = 3, Title = "Lalka",     Isbn = "978-83-7327-000-8", PublicationDate = new(1890, 1, 1), NumPages = 720, Price = 29.50m, Author = "Bolesław Prus" }
        );
    }
}