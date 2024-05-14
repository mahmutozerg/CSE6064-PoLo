using System.Security.Claims;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.Services.Mappers;
using SharedLibrary;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.User;
using SharedLibrary.Models.business;

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
        var user = await _userRepository.GetByIdAsync(userDeleteDto.Id);
        if (user == null)
            throw new Exception(ResponseMessages.UserNotFound);
        user.IsDeleted = true;
        await _unitOfWork.CommitAsync();

        return CustomResponseNoDataDto.Success(StatusCodes.Ok);


    }

    public async Task<CustomResponseNoDataDto> AddUserToCoursesAsync(AddUsersToCoursesDto dto,string updatedBy)
    {
        var user = await _userRepository.GetActiveUserWithCoursesByEMailAsync(dto.TeacherEmail);
        var errors = new List<string>();
        if (user is null)
            throw new Exception(ResponseMessages.UserNotFound);


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
        
        var user = await _userRepository.GetActiveUserWithCoursesByEMailAsync(dto.TeacherEmail);
        var errors = new List<string>();

        if (user is null)
            throw new Exception(ResponseMessages.UserNotFound);

        foreach (var coursesFullName in dto.CoursesFullNames)
        {
            var course = await _courseService.GetByIdAsync(coursesFullName);

            if (course.Data is null)
                errors.Add( coursesFullName + "Not found No changes made");
            else
                user.Courses.Remove(course.Data!);
            
        }
        if (errors.Count > 0)
            throw new Exception(string.Concat(errors.SelectMany(e => e)));
        
            
        user.UpdatedBy = updatedBy;
        
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return CustomResponseNoDataDto.Success(StatusCodes.Updated);    
    }

    public async Task<CustomResponseDto<List<AppUser>>> GetActiveUserWithCoursesByEMailAsync(string eMail)
    {
        var user = await _userRepository.SearchActiveUserWithCoursesByEMailAsync(eMail);

        return user == null
            ? throw new Exception(ResponseMessages.UserNotFound)
            : CustomResponseDto<List<AppUser>>.Success(user, StatusCodes.Ok);

    }

    public async Task<CustomResponseListDataDto<AppUser>> GetActiveUserWithCoursesByEMailByPageAsync(string eMail, string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(
                await _userRepository.GetActiveUserWithCoursesByEMailByPageAsync(eMail, intPage), StatusCodes.Ok); 

        throw new Exception(ResponseMessages.UserNotFound);
        
        
    }

    public async Task<CustomResponseDto<List<AppUser>>> GetUserWithCoursesByEMailAsync(string eMail)
    {
        var user = await _userRepository.GetUserWithCoursesByEMilAsync(eMail);

        return user == null
            ? throw new Exception(ResponseMessages.UserNotFound)
            : CustomResponseDto<List<AppUser>>.Success(user, StatusCodes.Ok);

    }

    public async Task<CustomResponseDto<List<AppUser>>> GetUserAsync(string eMail, string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseDto<List<AppUser>>.Success(await _userRepository.GetUserAsync(eMail,intPage),StatusCodes.Ok);
        
        throw new Exception(ResponseMessages.UserNotFound);
    }

    public async Task<CustomResponseListDataDto<AppUser>> GetActiveUserAsync(string eMail,string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
           return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetActiveUserAsync(eMail), StatusCodes.Ok);
        throw new Exception(ResponseMessages.UserNotFound);
    }

    public async Task<CustomResponseListDataDto<AppUser>> GetAllUsersByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetAllUsersByPageAsync(intPage),
                StatusCodes.Ok);

        throw new Exception(ResponseMessages.OutOfIndex);
    }
    
    public async Task<CustomResponseListDataDto<AppUser>> GetActiveUsersByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetActiveUsersByPageAsync(intPage),
                StatusCodes.Ok);

        throw new Exception(ResponseMessages.OutOfIndex);

    }

    public async Task<CustomResponseListDataDto<AppUser>> GetAllUsersWithCoursesByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetAllUsersWithCourseByPageAsync(intPage),
                StatusCodes.Ok);

        throw new Exception(ResponseMessages.OutOfIndex);

    }

    public async Task<CustomResponseListDataDto<AppUser>> GetActiveUsersWithCoursesByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(await _userRepository.GetActiveUsersWithCourseByPageAsync(intPage),
                StatusCodes.Ok);

        throw new Exception(ResponseMessages.OutOfIndex);
    }

    public async Task<CustomResponseDto<AppUser>> GetUserWithCoursesByIdAsync(string id)
    {
        var user = await _userRepository.GetUserWithCoursesByIdAsync(id);

        ArgumentNullException.ThrowIfNull(user);
        
        return CustomResponseDto<AppUser>.Success(user,
            StatusCodes.Ok);
    }

    public async Task<CustomResponseDto<AppUser>> GetUserWithCourseWithFilesWithResultByUserIdByCourseIdAsync(string userId, string courseId)
    {
        var user = await _userRepository.GetUserWithCourseWithFilesWithResultByUserIdByCourseIdAsync( userId,  courseId);
        ArgumentNullException.ThrowIfNull(user);
        
        return CustomResponseDto<AppUser>.Success(user,StatusCodes.Ok);
    }

    public async Task<CustomResponseDto<AppUser>> GetReadyResultCoursesAsync(string userId)
    {
        var user = await _userRepository.GetResultReadyCoursesAsync(userId);
        
        ArgumentNullException.ThrowIfNull(user);

        return CustomResponseDto<AppUser>.Success(user, StatusCodes.Ok);
    }

    public async Task<CustomResponseListDataDto<AppUser>> GetUserWithCoursesByEMailByPageAsync(string eMail, string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >= 0)
            return CustomResponseListDataDto<AppUser>.Success(
                await _userRepository.GetUserWithCoursesByEMailByPageAsync(eMail, intPage), StatusCodes.Ok); 

        throw new Exception(ResponseMessages.UserNotFound);    }

    public async Task<CustomResponseDto<AppUser>> GetUserWithCoursesById(string id)
    {
        var user = await _userRepository.GetUserWithCoursesById(id);
        
        ArgumentNullException.ThrowIfNull(user);
        
        return CustomResponseDto<AppUser>.Success(user,StatusCodes.Ok);
    }
}