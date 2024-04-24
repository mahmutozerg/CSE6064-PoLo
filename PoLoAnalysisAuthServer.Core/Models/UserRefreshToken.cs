namespace PoLoAnalysisAuthServer.Core.Models;

public class 
    UserRefreshToken
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}