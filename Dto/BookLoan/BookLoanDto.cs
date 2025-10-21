using LibraryAPI.Dto.Book;

namespace LibraryAPI.Dto.BookLoan;

public class BookLoanDto
{
    public int Id { get; set; }
    public BookDto Book { get; set; } = null!;
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}