using System.Security.Claims;
using LibraryAPI.Data;
using LibraryAPI.Dto.Book;
using LibraryAPI.Dto.BookLoan;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookLoansController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookLoanDto>>> GetMyLoans()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var loans = await context.BookLoans
            .Include(bl => bl.Book)
            .Where(bl => bl.UserId == userId)
            .Select(bl => new BookLoanDto
            {
                Id = bl.Id,
                LoanDate = bl.LoanDate,
                ReturnDate = bl.ReturnDate,
                Book = new BookDto
                {
                    Id = bl.Book.Id,
                    Title = bl.Book.Title,
                    Description = bl.Book.Description,
                    Author = bl.Book.Author
                }
            })
            .ToListAsync();

        return Ok(loans);
    }

    [HttpPost]
    public async Task<IActionResult> LoanBook([FromBody] BookLoanRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var bookExists = await context.Books.AnyAsync(b => b.Id == dto.BookId);
        if (!bookExists)
            return NotFound("Book not found");
        
        var isBookUnavailable = await context.BookLoans.AnyAsync(bl =>
            bl.BookId == dto.BookId &&
            bl.ReturnDate == null);

        if (isBookUnavailable)
            return BadRequest("Book is currently on loan.");

        var loan = new BookLoan
        {
            UserId = userId,
            BookId = dto.BookId,
            LoanDate = DateTime.UtcNow
        };

        context.BookLoans.Add(loan);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch("{id:int}/return")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var loan = await context.BookLoans
            .FirstOrDefaultAsync(bl => bl.Id == id && bl.UserId == userId);

        if (loan == null)
            return NotFound("Loan not found");

        if (loan.ReturnDate != null)
            return BadRequest("Book already returned");

        loan.ReturnDate = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return Ok();
    }
}