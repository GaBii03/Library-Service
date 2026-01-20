using LibraryService.Models.Entities;

namespace LibraryService.DL.Interfaces
{
    public interface IBookRepository
    {
        void AddBook(Book book); // Create

        Book? GetBookById(int id);  // Read

        List<Book> GetAllBooks(); // Read All

        void UpdateBook(Book book); // Update

        void DeleteBook(int id); // Delete
    }
}
