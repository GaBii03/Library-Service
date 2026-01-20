using FluentValidation;
using LibraryService.Models.Requests;

namespace LibraryService.Host.Validators
{
    public class AddBookValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required")
                .MaximumLength(100).WithMessage("Author must not exceed 100 characters");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required")
                .MaximumLength(20).WithMessage("ISBN must not exceed 20 characters");
        }
    }
}
