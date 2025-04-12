using FluentValidation;

namespace PP_BC_C_H_3_2.Validators
{
    public class DeleteEmployeeValidator : AbstractValidator<int>
    {
        public DeleteEmployeeValidator()
        {
            RuleFor(id => id).GreaterThan(0).WithMessage("ID must be greater than 0.");
        }
    }
}
