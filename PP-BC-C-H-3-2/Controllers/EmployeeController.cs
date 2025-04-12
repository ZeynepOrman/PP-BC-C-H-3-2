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

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] Employee employee)
        {
            var existingEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
                return NotFound();

            if (!string.IsNullOrEmpty(employee.Name))
                existingEmployee.Name = employee.Name;

            if (!string.IsNullOrEmpty(employee.Email))
                existingEmployee.Email = employee.Email;

            if (employee.Age > 0)
                existingEmployee.Age = employee.Age;

            return NoContent();
        }

        [HttpGet("list")]
        public IActionResult List([FromQuery] string name)
        {
            var filteredEmployees = employees.Where(e => e.Name.Contains(name)).ToList();
            return Ok(filteredEmployees);
        }
    }
}
