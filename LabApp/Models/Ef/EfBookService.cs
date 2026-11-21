namespace LabApp.Models.Ef;

public class EfBookService(AppDbContext context) : IBookService
{
    public IEnumerable<Book> FindAll()
    {
        return context.Books;
    }

    public Book? FindById(int id)
    {
        return context.Books.FirstOrDefault(b => b.Id == id);
    }

    public void Add(Book book)
    {
        context.Books.Add(book);
        context.SaveChanges();
    }

    public void Edit(Book book)
    {
        context.Books.Update(book);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var book = context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return;
        context.Books.Remove(book);
        context.SaveChanges();
    }
}