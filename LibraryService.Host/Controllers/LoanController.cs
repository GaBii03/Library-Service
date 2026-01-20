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
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly ILogger<LoanController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<AddLoanRequest> _validator;

        public LoanController(
            ILoanService loanService,
            ILogger<LoanController> logger,
            IMapper mapper,
            IValidator<AddLoanRequest> validator)
        {
            _loanService = loanService;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public ActionResult<List<Loan>> GetAllLoans()
        {
            try
            {
                var loans = _loanService.GetAllLoans();
                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all loans");
                return StatusCode(500, "An error occurred while retrieving loans");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Loan> GetLoanById(int id)
        {
            try
            {
                var loan = _loanService.GetLoanById(id);
                if (loan == null)
                {
                    return NotFound($"Loan with id {id} not found");
                }
                return Ok(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving loan with id {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the loan");
            }
        }

        [HttpGet("reader/{readerId}")]
        public ActionResult<List<Loan>> GetLoansByReaderId(int readerId)
        {
            try
            {
                var loans = _loanService.GetLoansByReaderId(readerId);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving loans for reader {ReaderId}", readerId);
                return StatusCode(500, "An error occurred while retrieving loans");
            }
        }

        [HttpGet("book/{bookId}")]
        public ActionResult<List<Loan>> GetLoansByBookId(int bookId)
        {
            try
            {
                var loans = _loanService.GetLoansByBookId(bookId);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving loans for book {BookId}", bookId);
                return StatusCode(500, "An error occurred while retrieving loans");
            }
        }

        [HttpPost]
        public ActionResult<Loan> AddLoan([FromBody] AddLoanRequest request)
        {
            if (request == null)
            {
                return BadRequest("Loan data is required");
            }

            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var loan = _loanService.BorrowBook(request.BookId, request.ReaderId, request.DueDate);
            if (loan == null)
            {
                return BadRequest("Book is not available or reader/book not found");
            }

            return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loan);
        }

        [HttpPost("{id}/return")]
        public IActionResult ReturnBook(int id)
        {
            try
            {
                var loan = _loanService.GetLoanById(id);
                if (loan == null)
                {
                    return NotFound($"Loan with id {id} not found");
                }

                if (loan.IsReturned)
                {
                    return BadRequest("Book has already been returned");
                }

                _loanService.ReturnBook(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning book for loan {Id}", id);
                return StatusCode(500, "An error occurred while returning the book");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLoan(int id)
        {
            try
            {
                var loan = _loanService.GetLoanById(id);
                if (loan == null)
                {
                    return NotFound($"Loan with id {id} not found");
                }

                _loanService.DeleteLoan(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting loan with id {Id}", id);
                return StatusCode(500, "An error occurred while deleting the loan");
            }
        }
    }
}
