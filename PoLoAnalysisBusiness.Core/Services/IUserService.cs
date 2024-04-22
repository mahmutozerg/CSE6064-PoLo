using System.Security.Claims;
using PoLoAnalysisAuthServer.Core.Models;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary.DTOs.User;
using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IUserService:IGenericService<AppUser>
{
    Task<CustomResponseDto<AppUser>> AddUserAsync(UserAddDto userAddDto, ClaimsIdentity claimsIdentity);

}