using LibraryService.BL.Interfaces;
using LibraryService.DL.Interfaces;
using LibraryService.Models.Entities;

namespace LibraryService.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void AddBook(Book book)
        {
            _bookRepository.AddBook(book);
        }

        public void DeleteBook(int id)
        {
            _bookRepository.DeleteBook(id);
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book? GetBookById(int id)
        {
            return _bookRepository.GetBookById(id);
        }

        public void UpdateBook(Book book)
        {
            _bookRepository.UpdateBook(book);
        }
    }
}
