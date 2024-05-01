namespace SharedLibrary.Configurations;

public class AppTokenOptions
{
    public List<String> Audience { get; set; } = new List<string>();
    public string Issuer { get; set; } = string.Empty;
    public int AccessTokenExpiration { get; set; } = int.MinValue;
    public int RefreshTokenExpiration { get; set; } = int.MinValue;
    public string SecurityKey { get; set; } = string.Empty;
}