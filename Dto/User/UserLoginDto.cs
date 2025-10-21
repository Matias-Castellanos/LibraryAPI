using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Dto.User;

public class UserLoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}