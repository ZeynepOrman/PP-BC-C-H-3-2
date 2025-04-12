using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PP_Project_Zeynep_O.Models;

namespace PP_Project_Zeynep_O.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> employees = new List<Employee>();
        private readonly IValidator<Employee> _validator;

        public EmployeeController(IValidator<Employee> validator)
        {
            _validator = validator;

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
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Employee employee)
        {
            var validationResult = _validator.Validate(employee);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            employee.Id = employees.Count + 1;
            employees.Add(employee);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Employee employee)
        {
            var existingEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
                return NotFound();

            var validationResult = _validator.Validate(employee);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Age = employee.Age;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();

            employees.Remove(employee);
            return NoContent();
        }
    }
}
