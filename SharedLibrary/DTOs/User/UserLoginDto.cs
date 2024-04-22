using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.User;

public class UserLoginDto
{
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Required(ErrorMessage = "Email field is required")]
    public string Email { get; set; } = "empty@testapp.com";

    [Required(ErrorMessage = "Password field is required")]
    public string Password { get; set; } = string.Empty;
}