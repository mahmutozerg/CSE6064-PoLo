using System.Security.Claims;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.DTO.Users;
using PoLoAnalysisBusiness.Services.Mappers;
using SharedLibrary;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisBusiness.Services.Services;

public class UserService:GenericService<AppUser>,IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICourseService _courseService;
    public UserService(IUserRepository repository, IUnitOfWork unitOfWork, ICourseService courseService) : base(repository, unitOfWork)
    {
        _userRepository = repository;
        _unitOfWork = unitOfWork;
        _courseService = courseService;
    }
    
    public async Task<CustomResponseDto<AppUser>> AddUserAsync(UserAddDto userAddDto,ClaimsIdentity claimsIdentity)
    {

        var createdBy = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userExist = await _userRepository.AnyAsync(u => u != null && u.EMail == userAddDto.Email && !u.IsDeleted);
        if (userExist)
            throw new Exception(ResponseMessages.UserAlreadyExist);

        var user = AppUserMapper.ToUser(userAddDto);
        user.CreatedBy = createdBy;
        user.UpdatedBy = createdBy;
        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();
        return CustomResponseDto<AppUser>.Success(user,StatusCodes.Created);

    }

    public async Task<CustomResponseNoDataDto> DeleteUserAsync(UserDeleteDto userDeleteDto)
    {
        var user = await _userRepository.GetById(userDeleteDto.Id);
        if (user == null)
            throw new Exception(ResponseMessages.UserNotFound);
        user.IsDeleted = true;
        await _unitOfWork.CommitAsync();

        return CustomResponseNoDataDto.Success(StatusCodes.Ok);


    }

    public async Task<CustomResponseNoDataDto> AddUserToCoursesAsync(AddUsersToCoursesDto dto,string updatedBy)
    {
        var user = await _userRepository.GetActiveUserWithCoursesByEmailAsync(dto.TeacherEmail);
        var errors = new List<string>();
        if (user is null)
            throw new Exception("");

        foreach (var coursesFullName in dto.CoursesFullNames)
        {
            var course = await _courseService.GetByIdAsync(coursesFullName);

            if (course.Data is null)
                errors.Add( coursesFullName + "Not found No changes made");
            else
                user.Courses.Add(course.Data!);
            
        }

        if (errors.Count > 0)
            throw new Exception(string.Concat(errors.SelectMany(e => e)));
            
        
        user.UpdatedBy = updatedBy;
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return CustomResponseNoDataDto.Success(StatusCodes.Updated);
    }

    public async Task<CustomResponseNoDataDto> RemoveUserFromCourseAsync(RemoveUserFromCourseDto dto, string updatedBy)
    {
        var user = await _userRepository.GetActiveUserWithCoursesByEmailAsync(dto.TeacherEmail);
        if (user is null)
            throw new Exception("");

        var targetCourseIndex = user.Courses.FindIndex(c => c.Id == dto.CourseFullName);
        
        if (targetCourseIndex < 0)
            throw new Exception(ResponseMessages.UserNotBelongCourse);

            
        user.Courses.RemoveAt(targetCourseIndex);
        user.UpdatedBy = updatedBy;
        
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return CustomResponseNoDataDto.Success(StatusCodes.Updated);    }

    public async Task<CustomResponseDto<AppUser>> GetActiveUserWithCoursesByEMailAsync(string eMail)
    {
        var user = await _userRepository.GetActiveUserWithCoursesByEmailAsync(eMail);

        return user == null
            ? throw new Exception(ResponseMessages.UserNotFound)
            : CustomResponseDto<AppUser>.Success(user, StatusCodes.Ok);

    }

    public async Task<CustomResponseDto<AppUser>> GetUserWithCoursesByEMailAsync(string eMail)
    {
        var user = await _userRepository.GetUserWithCoursesByEmailAsync(eMail);

        return user == null
            ? throw new Exception(ResponseMessages.UserNotFound)
            : CustomResponseDto<AppUser>.Success(user, StatusCodes.Ok);

    }
    public async Task<CustomResponseListDataDto<AppUser>> GetAllUsersByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetAllUsersByPage(intPage),
                StatusCodes.Ok);
                
        return CustomResponseListDataDto<AppUser>.Fail(ResponseMessages.OutOfIndex,StatusCodes.BadRequest);

    }
    
    public async Task<CustomResponseListDataDto<AppUser>> GetActiveUsersByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetActiveUsersByPage(intPage),
                StatusCodes.Ok);
                
        return CustomResponseListDataDto<AppUser>.Fail(ResponseMessages.OutOfIndex,StatusCodes.BadRequest);

    }

    public async Task<CustomResponseListDataDto<AppUser>> GetAllUsersWithCoursesByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetAllUsersWithCourseByPage(intPage),
                StatusCodes.Ok);
                
        return CustomResponseListDataDto<AppUser>.Fail(ResponseMessages.OutOfIndex,StatusCodes.BadRequest);
        
    }

    public async Task<CustomResponseListDataDto<AppUser>> GetActiveUsersWithCoursesByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetActiveUsersWithCourseByPage(intPage),
                StatusCodes.Ok);
                
        return CustomResponseListDataDto<AppUser>.Fail(ResponseMessages.OutOfIndex,StatusCodes.BadRequest);    
    }
}