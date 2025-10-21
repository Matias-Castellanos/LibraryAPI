using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(500)]
    public string Description { get; set; } = null!;
    
    public string? Author { get; set; }

    [JsonIgnore]
    public List<BookLoan> BookLoans { get; set; } = [];
}