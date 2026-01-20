using LibraryService.BL.Interfaces;
using LibraryService.BL.Services;
using LibraryService.DL.Interfaces;
using LibraryService.Models.Entities;
using Moq;

namespace LibraryService.Tests
{
    public class ReaderServiceTests
    {
        private readonly Mock<IReaderRepository> _readerRepositoryMock;
        private readonly IReaderService _readerService;

        public ReaderServiceTests()
        {
            _readerRepositoryMock = new Mock<IReaderRepository>();
            _readerService = new ReaderService(_readerRepositoryMock.Object);
        }

        [Fact]
        public void Add_ShouldGenerateId_WhenIdIsZero()
        {
            // Arrange
            var reader = new Reader
            {
                Id = 0,
                Name = "Test Reader",
                Email = "test@example.com"
            };

            var existingReaders = new List<Reader>
            {
                new Reader { Id = 1, Name = "Reader 1", Email = "reader1@example.com" },
                new Reader { Id = 2, Name = "Reader 2", Email = "reader2@example.com" }
            };

            _readerRepositoryMock.Setup(r => r.GetAll()).Returns(existingReaders);

            // Act
            _readerService.Add(reader);

            // Assert
            Assert.Equal(3, reader.Id);
            _readerRepositoryMock.Verify(r => r.Add(It.Is<Reader>(rd => rd.Id == 3)), Times.Once);
        }

        [Fact]
        public void Add_ShouldUseExistingId_WhenIdIsNotZero()
        {
            // Arrange
            var reader = new Reader
            {
                Id = 5,
                Name = "Test Reader",
                Email = "test@example.com"
            };

            // Act
            _readerService.Add(reader);

            // Assert
            Assert.Equal(5, reader.Id);
            _readerRepositoryMock.Verify(r => r.Add(It.Is<Reader>(rd => rd.Id == 5)), Times.Once);
        }

        [Fact]
        public void Add_ShouldNotAdd_WhenReaderIsNull()
        {
            // Act
            _readerService.Add(null);

            // Assert
            _readerRepositoryMock.Verify(r => r.Add(It.IsAny<Reader>()), Times.Never);
        }

        [Fact]
        public void GetAll_ShouldReturnAllReaders()
        {
            // Arrange
            var expectedReaders = new List<Reader>
            {
                new Reader { Id = 1, Name = "Reader 1", Email = "reader1@example.com" },
                new Reader { Id = 2, Name = "Reader 2", Email = "reader2@example.com" }
            };

            _readerRepositoryMock.Setup(r => r.GetAll()).Returns(expectedReaders);

            // Act
            var result = _readerService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _readerRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_ShouldReturnReader_WhenReaderExists()
        {
            // Arrange
            var readerId = 1;
            var expectedReader = new Reader
            {
                Id = readerId,
                Name = "Test Reader",
                Email = "test@example.com"
            };

            _readerRepositoryMock.Setup(r => r.GetById(readerId)).Returns(expectedReader);

            // Act
            var result = _readerService.GetById(readerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(readerId, result.Id);
            _readerRepositoryMock.Verify(r => r.GetById(readerId), Times.Once);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenReaderDoesNotExist()
        {
            // Arrange
            var readerId = 999;
            _readerRepositoryMock.Setup(r => r.GetById(readerId)).Returns((Reader?)null);

            // Act
            var result = _readerService.GetById(readerId);

            // Assert
            Assert.Null(result);
            _readerRepositoryMock.Verify(r => r.GetById(readerId), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            var readerId = 1;

            // Act
            _readerService.Delete(readerId);

            // Assert
            _readerRepositoryMock.Verify(r => r.Delete(readerId), Times.Once);
        }
    }
}
