using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PP_BC_C_H_3_2.Models;

namespace PP_BC_C_H_3_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private static List<Book> books = new List<Book>();
        private readonly IValidator<int> _getByIdValidator;
        private readonly IValidator<Book> _createValidator;
        private readonly IValidator<Book> _updateValidator;
        private readonly IValidator<int> _deleteValidator;

        public BookController(
            IValidator<int> getByIdValidator,
            IValidator<Book> createValidator,
            IValidator<Book> updateValidator,
            IValidator<int> deleteValidator)
        {
            _getByIdValidator = getByIdValidator;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;

            // Add dummy data
            books.Add(new Book { Id = 1, Name = "John Doe", Email = "john.doe@example.com", AccountNumber = "123456", Age = 30 });
            books.Add(new Book { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", AccountNumber = "654321", Age = 25 });
            books.Add(new Book { Id = 3, Name = "Alice Johnson", Email = "alice.johnson@example.com", AccountNumber = "789012", Age = 28 });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var validationResult = _getByIdValidator.Validate(id);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var book = books.FirstOrDefault(e => e.Id == id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateBook createBook)
        {
            var validationResult = _createValidator.Validate(new Book
            {
                Name = createBook.Name,
                Email = createBook.Email,
                AccountNumber = createBook.AccountNumber,
                Age = createBook.Age
            });

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // Generate a new ID based on the last existing ID
            int newId = books.Any() ? books.Max(e => e.Id) + 1 : 1;

            // Map CreateBook to Book
            var book = new Book
            {
                Id = newId,
                Name = createBook.Name,
                Email = createBook.Email,
                AccountNumber = createBook.AccountNumber,
                Age = createBook.Age
            };

            books.Add(book);
            return Ok(book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Book book)
        {
            var validationResult = _updateValidator.Validate(book);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var existingBook = books.FirstOrDefault(e => e.Id == id);
            if (existingBook == null)
                return NotFound();

            existingBook.Name = book.Name;
            existingBook.Email = book.Email;
            existingBook.Age = book.Age;

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var validationResult = _deleteValidator.Validate(id);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var book = books.FirstOrDefault(e => e.Id == id);
            if (book == null)
                return NotFound();

            books.Remove(book);
            return Ok();
        }
    }
}
