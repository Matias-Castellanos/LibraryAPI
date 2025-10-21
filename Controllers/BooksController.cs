using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Dto.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await context.Books
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Author = b.Author
                })
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await context.Books
                .Where(b => b.Id == id)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Author = b.Author
                })
                .FirstOrDefaultAsync();

            if (book == null) 
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDto>> Create(BookCreateDto dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var book = new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                Author = dto.Author
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            var result = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author
            };

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, BookCreateDto dto)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null) 
                return NotFound();

            book.Title = dto.Title;
            book.Description = dto.Description;
            book.Author = dto.Author;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null) 
                return NotFound();

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}