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
        private readonly IValidator<CreateBook> _createValidator;
        private readonly IValidator<Book> _updateValidator;
        private readonly IValidator<int> _deleteValidator;

        public BookController(
            IValidator<int> getByIdValidator,
            IValidator<CreateBook> createValidator,
            IValidator<Book> updateValidator,
            IValidator<int> deleteValidator)
        {
            _getByIdValidator = getByIdValidator;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;

            // Add dummy data
            books.Add(new Book { BookId = 1, GenreId = 101, Title = "Jane Eyre" });
            books.Add(new Book { BookId = 2, GenreId = 102, Title = "Güliverin Seyahatleri" });
            books.Add(new Book { BookId = 3, GenreId = 103, Title = "Dünyanın Merkezine Yolculuk" });
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

            var book = books.FirstOrDefault(e => e.BookId == id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateBook createBook)
        {
            // Validate the CreateBook object
            var validationResult = _createValidator.Validate(createBook);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // Generate a new ID based on the last existing ID
            int newId = books.Any() ? books.Max(e => e.BookId) + 1 : 1;

            // Map CreateBook to Book
            var book = new Book
            {
                BookId = newId,
                GenreId = createBook.GenreId,
                Title = createBook.Title
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

            var existingBook = books.FirstOrDefault(e => e.BookId == id);
            if (existingBook == null)
                return NotFound();

            existingBook.GenreId = book.GenreId;
            existingBook.Title = book.Title;

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var validationResult = _deleteValidator.Validate(id);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var book = books.FirstOrDefault(e => e.BookId == id);
            if (book == null)
                return NotFound();

            books.Remove(book);
            return Ok();
        }
    }
}
