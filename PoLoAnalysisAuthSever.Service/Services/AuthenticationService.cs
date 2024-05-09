using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PoLoAnalysisAuthServer.Core.Configurations;
using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.Models;
using PoLoAnalysisAuthServer.Core.Repositories;
using PoLoAnalysisAuthServer.Core.Services;
using SharedLibrary;
using SharedLibrary.DTOs.Client;
using SharedLibrary.DTOs.Tokens;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisAuthSever.Service.Services;

public class AuthenticationService:IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _refreshTokenService;
    private readonly List<ClientLoginDto >_tokenOptions;
    private readonly RoleManager<AppRole> _roleManager;


    public AuthenticationService(ITokenService tokenService, UserManager<User> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> refreshTokenService, IOptions<List<ClientLoginDto>> tokenOptions, RoleManager<AppRole> roleManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _refreshTokenService = refreshTokenService;
        _roleManager = roleManager;
        _tokenOptions = tokenOptions.Value;
    }

    
    public async Task<Response<TokenDto>> CreateTokenAsync(UserLoginDto loginDto)
    {
        ArgumentNullException.ThrowIfNull(loginDto);

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
            return Response<TokenDto>.Fail(ResponseMessages.WrongEmailOrPass, StatusCodes.NotFound,true);

        // if (! await _userManager.CheckPasswordAsync(user,loginDto.Password))
        //     return Response<TokenDto>.Fail("Email or password is wrong", 400,true);

        var token = await _tokenService.CreateTokenAsync(user);

        var userRefreshToken = await _refreshTokenService
            .Where(r => r != null && r.UserId == user.Id)
            .SingleOrDefaultAsync();

        if (userRefreshToken is null)
        {
            await _refreshTokenService.AddAsync(new()
                { UserId = user.Id, Token = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            userRefreshToken.Token = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }

        await _unitOfWork.CommitAsync();
        return Response<TokenDto>.Success(token,200);
    }

    public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string _refreshToken)
    {
        
        var refreshToken = await _refreshTokenService
            .Where(r => r != null && r.Token == _refreshToken)
            .SingleOrDefaultAsync();

        if (refreshToken is null)
            return Response<TokenDto>.Fail(ResponseMessages.NotFound, StatusCodes.NotFound,true);

        var user = await _userManager.FindByIdAsync(refreshToken.UserId);
        if (user is null)
            return Response<TokenDto>.Fail(ResponseMessages.NotFound, StatusCodes.NotFound,true);

        var token =await _tokenService.CreateTokenAsync(user);
        refreshToken.Token = token.RefreshToken;
        refreshToken.Expiration = token.RefreshTokenExpiration;

        await _unitOfWork.CommitAsync();

        return Response<TokenDto>.Success(token, StatusCodes.Ok);
        
    }
    public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        if (_tokenOptions == null || !_tokenOptions.Any())
            throw new Exception(ResponseMessages.NotFound);
        foreach (var configuredClient in _tokenOptions)
        {
            if (string.CompareOrdinal(configuredClient.Id, clientLoginDto.Id) == 0
                && string.CompareOrdinal(configuredClient.Secret, clientLoginDto.Secret) == 0)
            {
                var client = new Client()
                {
                    Id = clientLoginDto.Id,
                    Secret = clientLoginDto.Secret,
                    Audiences = ApiConstants.Aud
                };

                var clientTokenDto = _tokenService.CreateTokenByClient(client);
                return Response<ClientTokenDto>.Success(statusCode: 200, data: clientTokenDto);
            }
        }

        throw new Exception(ResponseMessages.NotFound);
    }
    
    public async Task<Response<NoDataDto>> RevokeRefreshToken(string _refreshToken)
    {
        var refreshToken = await _refreshTokenService
            .Where(r => r != null && r.Token == _refreshToken)
            .FirstOrDefaultAsync();
        
        if (refreshToken is null)
            return Response<NoDataDto>.Fail(ResponseMessages.NotFound, StatusCodes.NotFound,true);
        
        _refreshTokenService.Remove(refreshToken);
        await _unitOfWork.CommitAsync();


        return Response<NoDataDto>.Success(StatusCodes.Ok);

    }

    public async Task<Response<NoDataDto>> AddRole(string role)
    {
        var roleEntity = await _roleManager.FindByNameAsync(role);
        if (roleEntity is not null)
            return Response<NoDataDto>.Fail(ResponseMessages.AlreadyExists,StatusCodes.Duplicate,true);
        
        var result = await _roleManager.CreateAsync(new AppRole(role));
        
        return Response<NoDataDto>.Success(StatusCodes.Ok);

    }

    public async Task<UserRefreshToken> GetUserRefreshTokenByEmail(string userEmail)
    {
        var user =await _userManager.FindByNameAsync(userEmail.Split("@").First());
        if (user is null)
            throw new Exception(ResponseMessages.NotFound);

        var refreshToken = await _refreshTokenService
            .Where(u => u.UserId == user.Id)
            .FirstOrDefaultAsync();

        if (refreshToken is null)
            throw new Exception(ResponseMessages.NotFound);

        return refreshToken;

    }
}