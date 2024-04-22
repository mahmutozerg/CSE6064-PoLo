using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.User;

public class UserToTeacherRoleDto
{
    [Required(ErrorMessage = "UserMail field is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string UserMail { get; set; } = string.Empty;
}