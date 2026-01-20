using FluentValidation;
using LibraryService.Models.Requests;

namespace LibraryService.Host.Validators
{
    public class AddReaderValidator : AbstractValidator<AddReaderRequest>
    {
        public AddReaderValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be a valid email address")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters");
        }
    }
}
