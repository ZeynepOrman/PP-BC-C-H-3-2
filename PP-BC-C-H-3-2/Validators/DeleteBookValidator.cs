using FluentValidation;

namespace PP_BC_C_H_3_2.Validators
{
    public class DeleteBookValidator : AbstractValidator<int>
    {
        public DeleteBookValidator()
        {
            RuleFor(id => id).GreaterThan(0).WithMessage("ID must be greater than 0.");
        }
    }
}
