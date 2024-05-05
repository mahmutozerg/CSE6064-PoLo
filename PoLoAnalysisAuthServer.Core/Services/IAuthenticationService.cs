using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.Models;
using SharedLibrary.DTOs.Client;
using SharedLibrary.DTOs.Tokens;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisAuthServer.Core.Services;

public interface IAuthenticationService
{
    Task<Response<TokenDto>> CreateTokenAsync(UserLoginDto loginDto);

    Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

    Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

    Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    Task<Response<NoDataDto>> AddRole(string role);

    Task<UserRefreshToken> GetUserRefreshTokenByEmail(string userEmail);


}