namespace PoLoAnalysisAuthServer.Core.DTOs.Client;

public class ClientTokenDto
{
    public string AccesToken { get; set; }
    public DateTime AccesTokenExpiration { get; set; }
}