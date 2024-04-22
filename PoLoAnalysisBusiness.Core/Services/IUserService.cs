using System.Security.Claims;
using PoLoAnalysisAuthServer.Core.Models;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.User;
using SharedLibrary.Models;
using NoDataDto = PoLoAnalysisAuthServer.Core.DTOs.NoDataDto;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IUserService:IGenericService<AppUser>
{
    Task<CustomResponseDto<AppUser>> AddUserAsync(UserAddDto userAddDto, ClaimsIdentity claimsIdentity);
    Task<CustomResponseNoDataDto> DeleteUserAsync(UserDeleteDto userDeleteDto);

}