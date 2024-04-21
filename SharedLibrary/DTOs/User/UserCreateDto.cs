using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.User;

public class UserCreateDto
{
    [EmailAddress]
    [Required]
    public string EMail { get; set; }

    [Required]
    public string Passwd { get; set; }

    public string? Name { get; set; }
    public string? LastName { get; set; }

}