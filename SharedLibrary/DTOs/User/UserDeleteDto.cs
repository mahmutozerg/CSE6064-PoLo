using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.User;

public class UserDeleteDto
{
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; } 
}