using LibraryService.BL.Interfaces;
using LibraryService.Models.Entities;
using LibraryService.Models.Requests;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly IReaderService _readerService;
        private readonly ILogger<ReaderController> _logger;
        private readonly IMapper _mapper;

        private readonly IValidator<AddReaderRequest> _validator;

        public ReaderController(
            IReaderService readerService,
            ILogger<ReaderController> logger,
            IMapper mapper,
            IValidator<AddReaderRequest> validator)
        {
            _readerService = readerService;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet(nameof(GetAllReaders))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetAllReaders()
        {
            var readers =
                _readerService.GetAll();

            if (readers?.Count == 0) return NoContent();

            return Ok(readers);
        }

        [HttpGet(nameof(GetReaderById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetReaderById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than zero.");
            }

            var reader =
                _readerService.GetById(id);

            if (reader == null) return NotFound();

            return Ok(reader);
        }

        [HttpPost(nameof(AddReader))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddReader([FromBody] AddReaderRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Reader cannot be null.");
            }

            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            
            
            var reader = _mapper.Map<Reader>(request);

            if (reader == null) return BadRequest("Mapping failed.");

            _readerService.Add(reader);

            return Ok();
        }

        [HttpDelete(nameof(DeleteReader))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteReader(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than zero.");
            }

            _readerService.Delete(id);

            return Ok();
        }
    }
}
