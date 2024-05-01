using PoLoAnalysisAuthServer.Core.Configurations;
using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.DTOs.Client;
using PoLoAnalysisAuthServer.Core.Models;
using SharedLibrary.DTOs.Tokens;

namespace PoLoAnalysisAuthServer.Core.Services;

public interface ITokenService
{
    Task<TokenDto> CreateTokenAsync(User user);
    ClientTokenDto CreateTokenByClient(Client client);

}