using System.Security.Claims;
using PoLoAnalysisAuthServer.Core.Models;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.DTO.Responses;
using PoLoAnalysisBusiness.Services.Mappers;
using SharedLibrary;
using SharedLibrary.DTOs.User;
using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Services.Services;

public class UserService:GenericService<AppUser>,IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUserRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
    {
        _userRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CustomResponseDto<AppUser>> AddUserAsync(UserAddDto userAddDto,ClaimsIdentity claimsIdentity)
    {

        var createdBy = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userExist = await _userRepository.AnyAsync(u => u != null && u.EMail == userAddDto.Email);
        if (userExist)
            throw new Exception(ResponseMessages.UserAlreadyExist);

        var user = AppUserMapper.ToUser(userAddDto);
        user.CreatedBy = createdBy;
        user.UpdatedBy = createdBy;
        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();
        return CustomResponseDto<AppUser>.Success(user,StatusCodes.Created);

    }
}