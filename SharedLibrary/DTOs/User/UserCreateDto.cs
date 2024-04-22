using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.User;

public class UserCreateDto
{
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Required(ErrorMessage = "Email field is required")]
    public string Email { get; set; } = "empty@testapp.com";
    
    [Required(ErrorMessage = "Password field is required")]
    public string Password { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

}