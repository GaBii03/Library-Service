using FluentValidation;
using LibraryService.Models.Requests;

namespace LibraryService.Host.Validators
{
    public class AddLoanValidator : AbstractValidator<AddLoanRequest>
    {
        public AddLoanValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("BookId must be greater than zero");

            RuleFor(x => x.ReaderId)
                .GreaterThan(0).WithMessage("ReaderId must be greater than zero");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("DueDate is required")
                .GreaterThan(DateTime.Now).WithMessage("DueDate must be in the future");
        }
    }
}
