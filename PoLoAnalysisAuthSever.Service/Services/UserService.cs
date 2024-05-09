using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.Models;
using PoLoAnalysisAuthServer.Core.Repositories;
using PoLoAnalysisAuthServer.Core.Services;
using SharedLibrary;
using SharedLibrary.DTOs.Client;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.Tokens;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisAuthSever.Service.Services;

public class UserService : GenericService<User>, IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IGenericRepository<User> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAuthenticationService _authenticationService;
    private readonly IAuthenticationService _authService;
    private readonly List<ClientLoginDto> _clientTokenOptions;

    public UserService(UserManager<User> userManager, IGenericRepository<User> repository, IUnitOfWork unitOfWork,
        RoleManager<AppRole> roleManager, IAuthenticationService authenticationService,
        IOptions<List<ClientLoginDto>> clientTokenOptions, IAuthenticationService authService) : base(repository, unitOfWork)
    {
        _userManager = userManager;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _authenticationService = authenticationService;
        _authService = authService;
        _clientTokenOptions = clientTokenOptions.Value;
    }

    public async Task<Response<User>> CreateUserAsync(UserCreateDto createUserDto)
    {

        var user = new User
        {
            Id = Guid.NewGuid().ToString(), 
            Email = createUserDto.Email,
            UserName = createUserDto.Email.Split("@")[0],
            CreatedAt = DateTime.Now, CreatedBy = "System"
        };
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


        return Response<User>.Success(user, 200);
    }


    private async Task SendReqToBusinessApiAddById(User user, UserCreateDto createUserDto)
    {
        using (var client = new HttpClient())
        {
            const string url = ApiConstants.BusinessApiIp + "/api/User/AddById";

            var requestData = new UserAddDto()
            {
                Id = user.Id,
                Email = user.Email,
                Name = createUserDto.FirstName,
                LastName = createUserDto.LastName
            };

            var jsonData = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var clientToken = _authenticationService.CreateTokenByClient(
                new ClientLoginDto()
                {
                    Id = _clientTokenOptions[0].Id,
                    Secret = _clientTokenOptions[0].Secret
                }
            );
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken.Data.AccesToken);
            var response = await client.PostAsync(url, content);
            if (response.StatusCode != HttpStatusCode.Created)
                throw new Exception(ResponseMessages.BusinessApiOutOfReach);
        }
    }

    public async Task<Response<User>> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user is null ? 
            Response<User>.Fail(ResponseMessages.NotFound, StatusCodes.NotFound, true) 
            : Response<User>.Success(user,StatusCodes.Ok);
    }

    public async Task<Response<User>> GetUserByEmailAsync(string eMail)
    {
        var user = await _userManager.FindByEmailAsync(eMail);
        return user is null ? Response<User>.Fail(ResponseMessages.NotFound, StatusCodes.NotFound, true) : Response<User>.Success(user, 200);
    }

    public async Task<Response<NoDataDto>> Remove(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Response<NoDataDto>.Fail(ResponseMessages.NotFound, StatusCodes.NotFound, true);

        _repository.Remove(user);
        await _unitOfWork.CommitAsync();

        await SendDeleteReqToBusinessApi(user);
        return Response<NoDataDto>.Success(200);
    }

    public async Task<Response<NoDataDto>> AddRoleToUser(string userEmail, string roleName)
    {
        var user = await _userManager.FindByNameAsync(userEmail.Split("@").First());

        if (user is null)
            return Response<NoDataDto>.Fail(ResponseMessages.NotFound, StatusCodes.NotFound, true);

        var role = await _roleManager.FindByNameAsync(roleName);

        if (role is null)
            return Response<NoDataDto>.Fail(ResponseMessages.NotFound, StatusCodes.NotFound, true);

        await _userManager.AddToRoleAsync(user, roleName);

        return Response<NoDataDto>.Success(201);

    }

    public async Task<CustomResponseListDataDto<User>> GetAllUsersByPage(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (!res)
            throw new Exception(ResponseMessages.OutOfIndex);
        var users = await _userManager
            .Users
            .Skip(12*intPage)
            .Take(12)
            .ToListAsync();
        return CustomResponseListDataDto<User>.Success(users, StatusCodes.Ok);
    }

    public async Task SendDeleteReqToBusinessApi(User user)
    {
        using var client = new HttpClient();
        const string url = ApiConstants.BusinessApiIp + "/api/User/DeleteById";

        var requestData = new UserDeleteDto()
        {
            Id = user.Id,

        };

        var jsonData = JsonConvert.SerializeObject(requestData);

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var clientToken = _authenticationService.CreateTokenByClient(
            new ClientLoginDto()
            {
                Id = _clientTokenOptions[0].Id,
                Secret = _clientTokenOptions[0].Secret
            }
        );
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken.Data.AccesToken);
        var response = await client.PostAsync(url, content);
        if (response.StatusCode != HttpStatusCode.OK)
            throw new Exception(ResponseMessages.BusinessApiOutOfReach);
    }


}