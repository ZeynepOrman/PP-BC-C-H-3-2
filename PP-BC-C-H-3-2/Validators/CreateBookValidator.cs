using FluentValidation;
using PP_BC_C_H_3_2.Models;

namespace PP_BC_C_H_3_2.Validators
{
    public class CreateBookValidator : AbstractValidator<CreateBook>
    {
        public CreateBookValidator()
        {
            RuleFor(x => x.GenreId).GreaterThan(0).WithMessage("GenreId must be greater than 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty!")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters!");
        }
    }
}
