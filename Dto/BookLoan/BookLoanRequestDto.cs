using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Dto.BookLoan;

public class BookLoanRequestDto
{
    [Required]
    public int BookId { get; set; }
}