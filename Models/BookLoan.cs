using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class BookLoan
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    
    [Required]
    public DateTime LoanDate { get; set; } = DateTime.UtcNow;

    public DateTime? ReturnDate { get; set; }
}