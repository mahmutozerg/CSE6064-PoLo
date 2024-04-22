using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.Models;
using PoLoAnalysisAuthServer.Core.Repositories;
using PoLoAnalysisAuthServer.Core.Services;
using SharedLibrary;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisAuthSever.Service.Services;

public class UserService:GenericService<User>,IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IGenericRepository<User> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly List<ClientLoginDto>_clientTokenOptions;
     public UserService(UserManager<User> userManager, IGenericRepository<User> repository, IUnitOfWork unitOfWork, RoleManager<AppRole> roleManager, IAuthenticationService authenticationService, IOptions<List<ClientLoginDto>> clientTokenOptions, ITokenService tokenService) :base(repository, unitOfWork)
    {
        _userManager = userManager;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _authenticationService = authenticationService;
        _tokenService = tokenService;
        _clientTokenOptions = clientTokenOptions.Value;
    }

    public async Task<Response<User>> CreateUserAsync(UserCreateDto createUserDto)
    {
        var user = new User {Id = Guid.NewGuid().ToString(),Email = createUserDto.Email, UserName = createUserDto.Email.Split("@")[0],CreatedAt = DateTime.Now,CreatedBy = "System"};
        var result = await _userManager.CreateAsync(user, createUserDto.Password);
        
        if (!result.Succeeded)
            return Response<User>.Fail(result.Errors.Select(x => x.Description).ToList(), 409);
        
        /*
         * This is for creating the same user in business database
         * when a new user created we are sending the request to the business api
         * For that we need a bearer token(client token) which we are going to get from authentication service
         * User/AddById endpoint is authorized with a policy according to that so only a authserver client can reach there
         */

        await SendReqToBusinessApiAddById(user, createUserDto);
        
        
        return Response<User>.Success(user,200);
    }

    
    private async Task  SendReqToBusinessApiAddById(User user, UserCreateDto createUserDto)
    {
        using (var client = new HttpClient())
        {
            const string url = APIConstants.BusinessAPIIp+"/api/User/AddById";
 
            var requestData = new UserAddDto()
            {
                Id = user.Id,
                Email = user.Email,
                Name = createUserDto.FirstName,
                LastName = createUserDto.LastName
            };

            var jsonData = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            
            var clientToken =  _authenticationService.CreateTokenByClient(
                new ClientLoginDto() 
                {   
                    Id = _clientTokenOptions[0].Id,
                    Secret = _clientTokenOptions[0].Secret
                }
            );
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken.Data.AccesToken);
            var response = await client.PostAsync(url,content);
            if (response.StatusCode != HttpStatusCode.Created)
                throw new Exception("Authserver cannot reach api  ");
        }
    }
    public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return Response<UserAppDto>.Fail("Username does not exist", 404,true);
        
        return Response<UserAppDto>.Success(new UserAppDto()
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            
        }, 200);
    }

    public async Task<Response<NoDataDto>> Remove(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Response<NoDataDto>.Fail("User id does not exist",404,true);
        
        _repository.Remove(user);
        await _unitOfWork.CommitAsync();

        return Response<NoDataDto>.Success(200);
    }

    public async Task<Response<NoDataDto>> AddRoleToUser(string userEmail, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is null)
            return Response<NoDataDto>.Fail("User not found", 404, true);
        
        var role = await _roleManager.FindByNameAsync(roleName);
        
        if (role is null)
            return Response<NoDataDto>.Fail("role not found", 404, true);
        
        await _userManager.AddToRoleAsync(user, roleName);

        return Response<NoDataDto>.Success(201);

    }

    public async Task<Response<NoDataDto>> UpdateUser(AppUserUpdateDto appUserUpdateDto,ClaimsIdentity claimsIdentity)
    {
        var userId = "";
        if (string.IsNullOrEmpty(appUserUpdateDto.Id))
        {
            userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        }
        else
        {
            userId = appUserUpdateDto.Id;
        }

        if (userId == null)
            return Response<NoDataDto>.Fail("User not found", 404, true);
        
        var userEntityExist = await _userManager.FindByNameAsync(appUserUpdateDto.Email.Split('@')[0]);
        var userEntityAgain = await _userManager.FindByIdAsync(userId);
        if (userEntityExist is not null && userEntityAgain is  null)
        {
            return Response<NoDataDto>.Fail("Email already taken", 409, true);

        }
        var userEntity = await _userManager.FindByIdAsync(userId);
        if (userEntity == null) 
            return Response<NoDataDto>.Fail("User not found", 404, true);
        
        // Normally we dont have username field but Identity forces us to have one
        // se i decided to  take user's mail to be email => test@example.com(email) => test(username)

        userEntity.Email = string.IsNullOrEmpty(appUserUpdateDto.Email) ? userEntity.Email: appUserUpdateDto.Email;
        userEntity.UserName = userEntity.Email.Split('@')[0];
        
        try
        {
            await _userManager.UpdateAsync(userEntity);
            return Response<NoDataDto>.Success(202);
        }
        catch
        {
            return Response<NoDataDto>.Fail("Email already exist", 409, true);

        }
        
    }

}