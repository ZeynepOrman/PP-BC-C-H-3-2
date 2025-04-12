using FluentValidation;
using PP_BC_C_H_3_2.Models;

namespace PP_BC_C_H_3_2.Validators
{
    public class UpdateEmployeeValidator : AbstractValidator<Employee>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("ID must be greater than 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty!");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Enter a valid email!");
            RuleFor(x => x.Age).InclusiveBetween(18, 65).WithMessage("Age must be between 18 and 65.");
            RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Account number cannot be empty!");
        }
    }
}
