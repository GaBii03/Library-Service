using LibraryService.Models.Entities;

namespace LibraryService.BL.Interfaces
{
    public interface IBookService
    {
        void AddBook(Book book); // Create

        Book? GetBookById(int id);  // Read

        List<Book> GetAllBooks(); // Read All

        void UpdateBook(Book book); // Update

        void DeleteBook(int id); // Delete
    }
}
