using LibraryService.BL.Interfaces;
using LibraryService.Models.Entities;
using LibraryService.Models.Requests;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        private readonly IMapper _mapper;

        private readonly IValidator<AddBookRequest> _validator;

        public BookController(IBookService bookService,
                            ILogger<BookController> logger,
                            IMapper mapper,
                            IValidator<AddBookRequest> validator)
        {
            _bookService = bookService;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetAllBooks()
        {
            try
            {
                var books = _bookService.GetAllBooks();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books");
                return StatusCode(500, "An error occurred while retrieving books");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBookById(int id)
        {
            try
            {
                var book = _bookService.GetBookById(id);
                if (book == null)
                {
                    return NotFound($"Book with id {id} not found");
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book with id {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the book");
            }
        }

        [HttpPost]
        public ActionResult AddBook([FromBody] AddBookRequest request)
        {
            if (request == null)
            {
                return BadRequest("Book data is required");
            }

            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var book = _mapper.Map<Book>(request);
            if (book.Id == 0)
            {
                var allBooks = _bookService.GetAllBooks();
                book.Id = allBooks.Count > 0 ? allBooks.Max(b => b.Id) + 1 : 1;
            }
            book.IsAvailable = true;

            _bookService.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest("Book data is required");
                }

                if (id != book.Id)
                {
                    return BadRequest("Book ID mismatch");
                }

                var existingBook = _bookService.GetBookById(id);
                if (existingBook == null)
                {
                    return NotFound($"Book with id {id} not found");
                }

                _bookService.UpdateBook(book);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with id {Id}", id);
                return StatusCode(500, "An error occurred while updating the book");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                var book = _bookService.GetBookById(id);
                if (book == null)
                {
                    return NotFound($"Book with id {id} not found");
                }

                _bookService.DeleteBook(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with id {Id}", id);
                return StatusCode(500, "An error occurred while deleting the book");
            }
        }
    }
}
