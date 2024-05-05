using SharedLibrary.DTOs.User;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Services.Mappers;

public class AppUserMapper
{
    public static AppUser ToUser(UserAddDto userAddDto)
    {
        return new AppUser()
        {
            Name = string.IsNullOrEmpty(userAddDto.Name) ? string.Empty: userAddDto.Name,
            LastName = string.IsNullOrEmpty(userAddDto.LastName) ? string.Empty: userAddDto.LastName,
            Id = userAddDto.Id,
            IsDeleted = false,
            EMail = userAddDto.Email,
        };
    }


}