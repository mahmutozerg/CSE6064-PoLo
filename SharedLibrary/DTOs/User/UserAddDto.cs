using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.User;

public class UserAddDto
{
    [Required(ErrorMessage = "Id field is required")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "MailAddress field is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string Email { get; set; } = string.Empty;
    

    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}