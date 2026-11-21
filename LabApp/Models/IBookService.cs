namespace LabApp.Models;

public interface IBookService
{
    IEnumerable<Book> FindAll();
    Book? FindById(int id);
    void Add(Book book);
    void Edit(Book book);
    void Delete(int id);
}