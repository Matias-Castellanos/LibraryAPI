using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public enum Role { Admin = 0, User = 1 }

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(6)]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public Role Role { get; set; }

    [JsonIgnore]
    public List<BookLoan> BookLoans { get; set; } = [];
}