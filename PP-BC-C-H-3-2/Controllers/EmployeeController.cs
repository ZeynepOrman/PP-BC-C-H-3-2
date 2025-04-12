using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PP_BC_C_H_3_2.Models;

namespace PP_BC_C_H_3_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> employees = new List<Employee>();
        private readonly IValidator<int> _getByIdValidator;
        private readonly IValidator<Employee> _createValidator;
        private readonly IValidator<Employee> _updateValidator;
        private readonly IValidator<int> _deleteValidator;

        public EmployeeController(
            IValidator<int> getByIdValidator,
            IValidator<Employee> createValidator,
            IValidator<Employee> updateValidator,
            IValidator<int> deleteValidator)
        {
            _getByIdValidator = getByIdValidator;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;

            // Add dummy data
            employees.Add(new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com", AccountNumber = "123456", Age = 30 });
            employees.Add(new Employee { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", AccountNumber = "654321", Age = 25 });
            employees.Add(new Employee { Id = 3, Name = "Alice Johnson", Email = "alice.johnson@example.com", AccountNumber = "789012", Age = 28 });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var validationResult = _getByIdValidator.Validate(id);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateEmployee createEmployee)
        {
            var validationResult = _createValidator.Validate(new Employee
            {
                Name = createEmployee.Name,
                Email = createEmployee.Email,
                AccountNumber = createEmployee.AccountNumber,
                Age = createEmployee.Age
            });

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // Generate a new ID based on the last existing ID
            int newId = employees.Any() ? employees.Max(e => e.Id) + 1 : 1;

            // Map CreateEmployee to Employee
            var employee = new Employee
            {
                Id = newId,
                Name = createEmployee.Name,
                Email = createEmployee.Email,
                AccountNumber = createEmployee.AccountNumber,
                Age = createEmployee.Age
            };

            employees.Add(employee);
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Employee employee)
        {
            var validationResult = _updateValidator.Validate(employee);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var existingEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
                return NotFound();

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Age = employee.Age;

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var validationResult = _deleteValidator.Validate(id);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();

            employees.Remove(employee);
            return Ok();
        }
    }
}
