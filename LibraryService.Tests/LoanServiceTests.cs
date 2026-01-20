using LibraryService.BL.Interfaces;
using LibraryService.BL.Services;
using LibraryService.DL.Interfaces;
using LibraryService.Models.Entities;
using Moq;

namespace LibraryService.Tests
{
    public class LoanServiceTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReaderRepository> _readerRepositoryMock;
        private readonly ILoanService _loanService;

        public LoanServiceTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _readerRepositoryMock = new Mock<IReaderRepository>();
            _loanService = new LoanService(
                _loanRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _readerRepositoryMock.Object);
        }

        [Fact]
        public void BorrowBook_ShouldReturnLoan_WhenBookIsAvailable()
        {
            // Arrange
            var bookId = 1;
            var readerId = 1;
            var dueDate = DateTime.Now.AddDays(14);

            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                IsAvailable = true
            };

            var reader = new Reader
            {
                Id = readerId,
                Name = "Test Reader",
                Email = "test@example.com"
            };

            _bookRepositoryMock.Setup(r => r.GetBookById(bookId)).Returns(book);
            _readerRepositoryMock.Setup(r => r.GetById(readerId)).Returns(reader);
            _loanRepositoryMock.Setup(r => r.GetAllLoans()).Returns(new List<Loan>());

            // Act
            var result = _loanService.BorrowBook(bookId, readerId, dueDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Book.Id);
            Assert.Equal(readerId, result.Reader.Id);
            Assert.False(result.IsReturned);
            Assert.False(book.IsAvailable); // Book should be marked as unavailable
            _bookRepositoryMock.Verify(r => r.UpdateBook(It.Is<Book>(b => !b.IsAvailable)), Times.Once);
            _loanRepositoryMock.Verify(r => r.AddLoan(It.IsAny<Loan>()), Times.Once);
        }

        [Fact]
        public void BorrowBook_ShouldReturnNull_WhenBookIsNotAvailable()
        {
            // Arrange
            var bookId = 1;
            var readerId = 1;
            var dueDate = DateTime.Now.AddDays(14);

            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                IsAvailable = false // Book is not available
            };

            var reader = new Reader
            {
                Id = readerId,
                Name = "Test Reader",
                Email = "test@example.com"
            };

            _bookRepositoryMock.Setup(r => r.GetBookById(bookId)).Returns(book);
            _readerRepositoryMock.Setup(r => r.GetById(readerId)).Returns(reader);

            // Act
            var result = _loanService.BorrowBook(bookId, readerId, dueDate);

            // Assert
            Assert.Null(result);
            _loanRepositoryMock.Verify(r => r.AddLoan(It.IsAny<Loan>()), Times.Never);
        }

        [Fact]
        public void BorrowBook_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 1;
            var readerId = 1;
            var dueDate = DateTime.Now.AddDays(14);

            _bookRepositoryMock.Setup(r => r.GetBookById(bookId)).Returns((Book?)null);

            // Act
            var result = _loanService.BorrowBook(bookId, readerId, dueDate);

            // Assert
            Assert.Null(result);
            _loanRepositoryMock.Verify(r => r.AddLoan(It.IsAny<Loan>()), Times.Never);
        }

        [Fact]
        public void BorrowBook_ShouldReturnNull_WhenReaderDoesNotExist()
        {
            // Arrange
            var bookId = 1;
            var readerId = 1;
            var dueDate = DateTime.Now.AddDays(14);

            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                IsAvailable = true
            };

            _bookRepositoryMock.Setup(r => r.GetBookById(bookId)).Returns(book);
            _readerRepositoryMock.Setup(r => r.GetById(readerId)).Returns((Reader?)null);

            // Act
            var result = _loanService.BorrowBook(bookId, readerId, dueDate);

            // Assert
            Assert.Null(result);
            _loanRepositoryMock.Verify(r => r.AddLoan(It.IsAny<Loan>()), Times.Never);
        }

        [Fact]
        public void ReturnBook_ShouldMarkLoanAsReturned_AndBookAsAvailable()
        {
            // Arrange
            var loanId = 1;
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                IsAvailable = false
            };

            var loan = new Loan
            {
                Id = loanId,
                Book = book,
                Reader = new Reader { Id = 1, Name = "Test Reader", Email = "test@example.com" },
                BorrowDate = DateTime.Now.AddDays(-7),
                DueDate = DateTime.Now.AddDays(7),
                IsReturned = false
            };

            _loanRepositoryMock.Setup(r => r.GetLoanById(loanId)).Returns(loan);

            // Act
            _loanService.ReturnBook(loanId);

            // Assert
            Assert.True(loan.IsReturned);
            Assert.NotNull(loan.ReturnDate);
            Assert.True(book.IsAvailable);
            _bookRepositoryMock.Verify(r => r.UpdateBook(It.Is<Book>(b => b.IsAvailable)), Times.Once);
            _loanRepositoryMock.Verify(r => r.UpdateLoan(It.Is<Loan>(l => l.IsReturned)), Times.Once);
        }

        [Fact]
        public void ReturnBook_ShouldNotUpdate_WhenLoanIsAlreadyReturned()
        {
            // Arrange
            var loanId = 1;
            var loan = new Loan
            {
                Id = loanId,
                Book = new Book { Id = 1, Title = "Test Book", IsAvailable = false },
                Reader = new Reader { Id = 1, Name = "Test Reader", Email = "test@example.com" },
                BorrowDate = DateTime.Now.AddDays(-7),
                DueDate = DateTime.Now.AddDays(7),
                IsReturned = true,
                ReturnDate = DateTime.Now.AddDays(-1)
            };

            _loanRepositoryMock.Setup(r => r.GetLoanById(loanId)).Returns(loan);

            // Act
            _loanService.ReturnBook(loanId);

            // Assert
            _bookRepositoryMock.Verify(r => r.UpdateBook(It.IsAny<Book>()), Times.Never);
            _loanRepositoryMock.Verify(r => r.UpdateLoan(It.IsAny<Loan>()), Times.Never);
        }

        [Fact]
        public void ReturnBook_ShouldNotUpdate_WhenLoanDoesNotExist()
        {
            // Arrange
            var loanId = 999;
            _loanRepositoryMock.Setup(r => r.GetLoanById(loanId)).Returns((Loan?)null);

            // Act
            _loanService.ReturnBook(loanId);

            // Assert
            _bookRepositoryMock.Verify(r => r.UpdateBook(It.IsAny<Book>()), Times.Never);
            _loanRepositoryMock.Verify(r => r.UpdateLoan(It.IsAny<Loan>()), Times.Never);
        }

        [Fact]
        public void GetLoansByReaderId_ShouldReturnLoansForReader()
        {
            // Arrange
            var readerId = 1;
            var expectedLoans = new List<Loan>
            {
                new Loan
                {
                    Id = 1,
                    Book = new Book { Id = 1, Title = "Book 1" },
                    Reader = new Reader { Id = readerId, Name = "Reader 1" },
                    IsReturned = false
                }
            };

            _loanRepositoryMock.Setup(r => r.GetLoansByReaderId(readerId)).Returns(expectedLoans);

            // Act
            var result = _loanService.GetLoansByReaderId(readerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _loanRepositoryMock.Verify(r => r.GetLoansByReaderId(readerId), Times.Once);
        }

        [Fact]
        public void GetLoansByBookId_ShouldReturnLoansForBook()
        {
            // Arrange
            var bookId = 1;
            var expectedLoans = new List<Loan>
            {
                new Loan
                {
                    Id = 1,
                    Book = new Book { Id = bookId, Title = "Book 1" },
                    Reader = new Reader { Id = 1, Name = "Reader 1" },
                    IsReturned = false
                }
            };

            _loanRepositoryMock.Setup(r => r.GetLoansByBookId(bookId)).Returns(expectedLoans);

            // Act
            var result = _loanService.GetLoansByBookId(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _loanRepositoryMock.Verify(r => r.GetLoansByBookId(bookId), Times.Once);
        }

        [Fact]
        public void GetAllLoans_ShouldReturnAllLoans()
        {
            // Arrange
            var expectedLoans = new List<Loan>
            {
                new Loan { Id = 1, Book = new Book { Id = 1 }, Reader = new Reader { Id = 1 }, IsReturned = false },
                new Loan { Id = 2, Book = new Book { Id = 2 }, Reader = new Reader { Id = 2 }, IsReturned = true }
            };

            _loanRepositoryMock.Setup(r => r.GetAllLoans()).Returns(expectedLoans);

            // Act
            var result = _loanService.GetAllLoans();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _loanRepositoryMock.Verify(r => r.GetAllLoans(), Times.Once);
        }

        [Fact]
        public void DeleteLoan_ShouldCallRepositoryDeleteLoan()
        {
            // Arrange
            var loanId = 1;

            // Act
            _loanService.DeleteLoan(loanId);

            // Assert
            _loanRepositoryMock.Verify(r => r.DeleteLoan(loanId), Times.Once);
        }
    }
}
