namespace PoLoAnalysisAuthServer.Core.DTOs;

public class ClientTokenDto
{
    public string AccesToken { get; set; }
    public DateTime AccesTokenExpiration { get; set; }
}