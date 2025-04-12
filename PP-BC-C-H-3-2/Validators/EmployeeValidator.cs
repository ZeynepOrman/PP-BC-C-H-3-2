using FluentValidation;
using PP_Project_Zeynep_O.Models;

namespace PP_Project_Zeynep_O.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Yaş boş bırakılamaz!");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir email giriniz!");
            RuleFor(x => x.Age).InclusiveBetween(18, 65).WithMessage("Yaş 18 ile 65 arasında olmalıdır.");
            RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Hesap numarası boş bırakılamaz!");
        }
    }
}
