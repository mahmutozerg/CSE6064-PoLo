using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.User;

public class CatsUserLogin
{
    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; } = "empty@testapp.com";

    [Required(ErrorMessage = "Password field is required")]
    public string Password { get; set; } = string.Empty;
}