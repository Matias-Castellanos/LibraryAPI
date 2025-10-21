using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Dto.Book;

public class BookCreateDto
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(500)]
    public string Description { get; set; } = null!;

    [MaxLength(100)]
    public string? Author { get; set; }
}