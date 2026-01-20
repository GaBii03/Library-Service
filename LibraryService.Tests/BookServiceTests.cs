using LibraryService.BL.Interfaces;
using LibraryService.BL.Services;
using LibraryService.DL.Interfaces;
using LibraryService.Models.Entities;
using Moq;

namespace LibraryService.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly IBookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookService = new BookService(_bookRepositoryMock.Object);
        }

        [Fact]
        public void AddBook_ShouldCallRepositoryAddBook()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                IsAvailable = true
            };

            // Act
            _bookService.AddBook(book);

            // Assert
            _bookRepositoryMock.Verify(r => r.AddBook(It.Is<Book>(b => b.Id == 1 && b.Title == "Test Book")), Times.Once);
        }

        [Fact]
        public void GetBookById_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var bookId = 1;
            var expectedBook = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                IsAvailable = true
            };

            _bookRepositoryMock.Setup(r => r.GetBookById(bookId)).Returns(expectedBook);

            // Act
            var result = _bookService.GetBookById(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
            Assert.Equal("Test Book", result.Title);
            _bookRepositoryMock.Verify(r => r.GetBookById(bookId), Times.Once);
        }

        [Fact]
        public void GetBookById_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 999;
            _bookRepositoryMock.Setup(r => r.GetBookById(bookId)).Returns((Book?)null);

            // Act
            var result = _bookService.GetBookById(bookId);

            // Assert
            Assert.Null(result);
            _bookRepositoryMock.Verify(r => r.GetBookById(bookId), Times.Once);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnAllBooks()
        {
            // Arrange
            var expectedBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "111", IsAvailable = true },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "222", IsAvailable = false }
            };

            _bookRepositoryMock.Setup(r => r.GetAllBooks()).Returns(expectedBooks);

            // Act
            var result = _bookService.GetAllBooks();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Book 1", result[0].Title);
            _bookRepositoryMock.Verify(r => r.GetAllBooks(), Times.Once);
        }

        [Fact]
        public void UpdateBook_ShouldCallRepositoryUpdateBook()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Updated Book",
                Author = "Updated Author",
                ISBN = "9999999999",
                IsAvailable = false
            };

            // Act
            _bookService.UpdateBook(book);

            // Assert
            _bookRepositoryMock.Verify(r => r.UpdateBook(It.Is<Book>(b => b.Id == 1 && b.Title == "Updated Book")), Times.Once);
        }

        [Fact]
        public void DeleteBook_ShouldCallRepositoryDeleteBook()
        {
            // Arrange
            var bookId = 1;

            // Act
            _bookService.DeleteBook(bookId);

            // Assert
            _bookRepositoryMock.Verify(r => r.DeleteBook(bookId), Times.Once);
        }
    }
}
