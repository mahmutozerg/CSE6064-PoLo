using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.Tokens;

public class CreateTokenByRefreshTokenDto
{
    [Required(ErrorMessage = "RefreshToken field is required")]
    public string RefreshToken { get; set; }
}