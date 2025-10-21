namespace LibraryAPI.Dto.Book;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Author { get; set; }
}